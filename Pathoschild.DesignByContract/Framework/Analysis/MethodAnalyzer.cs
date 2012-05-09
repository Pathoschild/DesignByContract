using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
	/// <summary>Reflects methods and properties for contract analysis.</summary>
	[Serializable]
	public class MethodAnalyzer : IMethodAnalyzer
	{
		/*********
		** Accessors
		*********/
		/// <summary>The singleton instance.</summary>
		public static MethodAnalyzer Instance = new MethodAnalyzer();


		/*********
		** Public methods
		*********/
		/// <summary>Analyze the contract annotations on a methods.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContract">Whether to inherit attributes from base types or interfaces.</param>
		public MethodAnalysis AnalyzeMethod(MethodBase method, bool inheritContract)
		{
			// analyze method contract
			string friendlyName = this.GetFriendlyName(method);
			var parameterPreconditions = this.GetParameterPreconditions(method, friendlyName, inheritContract);
			var returnPreconditions = this.GetReturnValuePreconditions(method, friendlyName, inheritContract);

			// analyze property contract
			PropertyInfo property = this.GetProperty(method);
			if (property != null)
			{
				// cascade annotations on the property to the getter/setter methods
				if (this.IsPropertyGetter(method))
					returnPreconditions = returnPreconditions.Union(this.GetReturnValuePreconditions(property, friendlyName, inheritContract));
				else
					parameterPreconditions = parameterPreconditions.Union(this.GetParameterPreconditions(property, friendlyName, inheritContract));
			}

			// return analysis
			return new MethodAnalysis
			{
				FriendlyName = friendlyName,
				ParameterPreconditions = parameterPreconditions.ToArray(),
				ReturnValuePreconditions = returnPreconditions.ToArray()
			};
		}


		/*********
		** Protected methods
		*********/
		/***
		** Method analysis
		***/
		/// <summary>Get a human-readable name representing the method or property being validated for use in exception messages.</summary>
		/// <param name="method">The method to analyze.</param>
		protected string GetFriendlyName(MemberInfo method)
		{
			return String.Format("{0}::{1}", method.DeclaringType.Name, method.Name);
		}

		/// <summary>Get whether a method returns a value.</summary>
		/// <param name="method">The method to analyze.</param>
		protected bool HasReturnValue(MethodBase method)
		{
			MethodInfo methodInfo = method as MethodInfo;
			return methodInfo != null && methodInfo.ReturnType != typeof(void);
		}

		/// <summary>Get whether a method is a property getter or setter.</summary>
		/// <param name="method">The method to analyze.</param>
		protected bool IsPropertyAccessor(MethodBase method)
		{
			MethodInfo methodInfo = method as MethodInfo;
			return methodInfo != null
				&& methodInfo.IsSpecialName
				&& (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"));
		}

		/// <summary>Get whether the method is a property setter.</summary>
		/// <param name="method">The method to analyze.</param>
		protected bool IsPropertyGetter(MethodBase method)
		{
			return this.IsPropertyAccessor(method)
				&& method.Name.StartsWith("get_");
		}

		/// <summary>Get the property for which this method is an accessor.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <returns>Returns the method's property, or <c>null</c> if it is not an accessor.</returns>
		protected PropertyInfo GetProperty(MethodBase method)
		{
			if (!this.IsPropertyAccessor(method))
				return null;

			string name = method.Name.Remove(0, 4);
			return method.DeclaringType
				.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		}

		/***
		** Attributes
		***/
		/// <summary>Get the custom attributes of a given type from a provider.</summary>
		/// <typeparam name="T">The type of the custom attributes.</typeparam>
		/// <param name="customAttributeProvider">The object from which to retrieve custom attributes.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<T> GetCustomAttributes<T>(ICustomAttributeProvider customAttributeProvider, bool inherit)
		{
			return customAttributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
		}

		/// <summary>Get the custom attributes of a given type from a provider.</summary>
		/// <typeparam name="T">The type of the custom attributes.</typeparam>
		/// <param name="property">The object from which to retrieve custom attributes.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<T> GetCustomAttributes<T>(MemberInfo property, bool inherit)
		{
			return property.GetCustomAttributes(typeof(T), inherit).Cast<T>();
		}

		/// <summary>Get the attributes applied to a method.</summary>
		/// <typeparam name="T">The attribute type to get.</typeparam>
		/// <param name="method">The method whose attributes to get.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		/// <param name="withReturnTypeAttributes">Whether to include attributes restricted to its return type.</param>
		protected IEnumerable<T> GetMethodAttributes<T>(MethodInfo method, bool inherit, bool withReturnTypeAttributes = false)
		{
			var attributes = this.GetCustomAttributes<T>(method, inherit);
			if (withReturnTypeAttributes)
				attributes = attributes.Union(this.GetCustomAttributes<T>(method.ReturnTypeCustomAttributes, inherit));
			return attributes;
		}

		/// <summary>Get the attributes applied to a parameter.</summary>
		/// <typeparam name="T">The attribute type to get.</typeparam>
		/// <param name="parameter">The parameter whose attributes to get.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<T> GetParameterAttributes<T>(ParameterInfo parameter, bool inherit)
		{
			return this.GetCustomAttributes<T>(parameter, inherit);
		}

		/***
		** Preconditions
		***/
		/// <summary>Get the contract requirements for each method parameter or property setter value.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="friendlyName">A human-readable name representing the method or property being validated for use in exception messages.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ParameterMetadata> GetParameterPreconditions(MethodBase method, string friendlyName, bool inherit)
		{
			return (
				from parameter in method.GetParameters()
				from IParameterPrecondition annotation in this.GetParameterAttributes<IParameterPrecondition>(parameter, inherit)
				let messageFormat = this.GetMessageFormatForParameter(friendlyName, parameter.Name)
				select new ParameterMetadata(parameter, annotation, messageFormat)
			);
		}

		/// <summary>Get the contract requirements for the property setter value.</summary>
		/// <param name="property">The property to analyze.</param>
		/// <param name="friendlyName">A human-readable name representing the method or property being validated for use in exception messages.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ParameterMetadata> GetParameterPreconditions(PropertyInfo property, string friendlyName, bool inherit)
		{
			if (!property.CanWrite)
				return new ParameterMetadata[0];

			string messageFormat = this.GetMessageFormatForParameter(friendlyName, "value");
			ParameterInfo parameter = property.GetSetMethod(true).GetParameters().Last(); // setter value (implicit last parameter)
			return (
				from annotation in this.GetCustomAttributes<IParameterPrecondition>(property, inherit)
				select new ParameterMetadata(parameter, annotation, messageFormat)
			);
		}

		/// <summary>Get the contract requirements on a method or property return value.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="friendlyName">A human-readable name representing the method or property being validated for use in exception messages.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ReturnValueMetadata> GetReturnValuePreconditions(MethodBase method, string friendlyName, bool inherit)
		{
			if (!this.HasReturnValue(method))
				return new ReturnValueMetadata[0];

			string messageFormat = this.GetMessageFormatForReturn(friendlyName);
			return this
				.GetMethodAttributes<IReturnValuePrecondition>(method as MethodInfo, inherit, true)
				.Select(attr => new ReturnValueMetadata(messageFormat, attr));
		}

		/// <summary>Get the contract requirements on a method or property return value.</summary>
		/// <param name="property">The method to analyze.</param>
		/// <param name="friendlyName">A human-readable name representing the method or property being validated for use in exception messages.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ReturnValueMetadata> GetReturnValuePreconditions(PropertyInfo property, string friendlyName, bool inherit)
		{
			string messageFormat = this.GetMessageFormatForReturn(friendlyName);
			return this
				.GetCustomAttributes<IReturnValuePrecondition>(property, inherit)
				.Select(attr => new ReturnValueMetadata(messageFormat, attr));
		}

		/// <summary>Get the suggested exception message format for a return value contract violation, where {0} is the message.</summary>
		/// <param name="friendlyMethodName">The friendly name of the method or property being validated.</param>
		protected string GetMessageFormatForReturn(string friendlyMethodName)
		{
			return String.Concat("Contract violation on return value of method '", friendlyMethodName, "': {0}");
		}

		/// <summary>Get the suggested exception message format for a parameter contract violation, where {0} is the message.</summary>
		/// <param name="friendlyMethodName">The friendly name of the method or property being validated.</param>
		/// <param name="parameterName">The name of the parameter being validated.</param>
		protected string GetMessageFormatForParameter(string friendlyMethodName, string parameterName)
		{
			return String.Concat("Contract violation on parameter '", parameterName, "' of method '", friendlyMethodName, "': {0}");
		}
	}
}
