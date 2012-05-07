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
			var parameterPreconditions = this.GetParameterPreconditions(method, inheritContract);
			var returnPreconditions = this.GetReturnValuePreconditions(method, inheritContract);

			// analyze property contract
			PropertyInfo property = this.GetProperty(method);
			if (property != null)
			{
				// cascade annotations on the property to the getter/setter methods
				if (this.IsPropertyGetter(method))
					returnPreconditions = returnPreconditions.Union(this.GetReturnValuePreconditions(property, inheritContract));
				else
					parameterPreconditions = parameterPreconditions.Union(this.GetParameterPreconditions(property, inheritContract));
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
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ParameterMetadata> GetParameterPreconditions(MethodBase method, bool inherit)
		{
			return (
				from metadata in method.GetParameters().Select((parameter, index) => new { parameter, index })
				from IParameterPrecondition annotation in this.GetParameterAttributes<IParameterPrecondition>(metadata.parameter, inherit)
				select new ParameterMetadata(metadata.parameter, metadata.index, annotation)
			);
		}

		/// <summary>Get the contract requirements for the property setter value.</summary>
		/// <param name="property">The property to analyze.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<ParameterMetadata> GetParameterPreconditions(PropertyInfo property, bool inherit)
		{
			if (!property.CanWrite)
				return new ParameterMetadata[0];

			ParameterInfo parameter = property.GetSetMethod(true).GetParameters().First();
			return (
				from annotation in this.GetCustomAttributes<IParameterPrecondition>(property, inherit)
				select new ParameterMetadata(parameter, 0, annotation)
			);
		}

		/// <summary>Get the contract requirements on a method or property return value.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<IReturnValuePrecondition> GetReturnValuePreconditions(MethodBase method, bool inherit)
		{
			return this.HasReturnValue(method)
				? this.GetMethodAttributes<IReturnValuePrecondition>(method as MethodInfo, inherit, true)
				: new IReturnValuePrecondition[0];
		}

		/// <summary>Get the contract requirements on a method or property return value.</summary>
		/// <param name="property">The method to analyze.</param>
		/// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
		protected IEnumerable<IReturnValuePrecondition> GetReturnValuePreconditions(PropertyInfo property, bool inherit)
		{
			return this.GetCustomAttributes<IReturnValuePrecondition>(property, inherit);
		}
	}
}
