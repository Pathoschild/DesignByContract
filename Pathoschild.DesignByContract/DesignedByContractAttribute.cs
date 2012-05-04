using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pathoschild.DesignByContract.Framework;
using PostSharp.Aspects;

namespace Pathoschild.DesignByContract
{
	/// <summary>Indicates that invocations of the target methods should be validated to ensure that their preconditions or postconditions are respected.</summary>
	/// <remarks>This aspect analyzes annotated code at compile time, serializes the reflection metadata into its cache, and intercepts method calls at runtime to check conditions.</remarks>
	[AttributeUsage((AttributeTargets)(ConditionTargets.All) | AttributeTargets.Class)]
	[Serializable]
	public class DesignedByContractAttribute : OnMethodBoundaryAspect
	{
		/*********
		** Accessors
		*********/
		/// <summary>A human-readable name representing the target being validated for use in exception messages.</summary>
		/// <example>For example, the friendly name for a method named "Hit" on a class named "Sword" might be "Sword::Hit".</example>
		public string FriendlyName { get; set; }

		/// <summary>Whether to inherit contract annotations defined on a base class or interface.</summary>
		public bool InheritContract { get; set; }

		/// <summary>The contract requirements that must be met before a method is executed.</summary>
		public IMethodPrecondition[] MethodPreconditions { get; set; }

		/// <summary>The contract requirements that must be met after a method is executed.</summary>
		public IMethodPostcondition[] MethodPostconditions { get; set; }

		/// <summary>The contract requirements for each method parameter or property setter value.</summary>
		public ParameterMetadata[] ParameterPreconditions { get; set; }
		
		/// <summary>The contract requirements on a method or property return value.</summary>
		public IReturnValuePrecondition[] ReturnValuePreconditions { get; set; }
		

		/*********
		** Public methods
		*********/
		/// <summary>Construct an annotation which indicates that invocations of the target methods should be validated to ensure that their preconditions or postconditions are respected.</summary>
		/// <param name="inheritContract">Whether to inherit the contract annotations defined on a base class or interface.</param>
		public DesignedByContractAttribute(bool inheritContract = true)
		{
			this.InheritContract = inheritContract;
		}

		/// <summary>Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked before any other build-time method.</summary>
		/// <param name="method">Method to which the current aspect is applied</param>
		/// <param name="aspectInfo">Reserved for future usage.</param>
		public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
		{
			// generate friendly method name
			this.FriendlyName = this.GetFriendlyName(method);

			// analyze contract conditions
			this.MethodPreconditions = this.GetMethodPreconditions(method, this.InheritContract);
			this.MethodPostconditions = this.GetMethodPostconditions(method, this.InheritContract);
			this.ParameterPreconditions = this.GetParameterPreconditions(method, this.InheritContract);
			this.ReturnValuePreconditions = this.GetReturnValuePreconditions(method, this.InheritContract);

			base.CompileTimeInitialize(method, aspectInfo);
		}

		/// <summary>Method executed <b>before</b> the body of methods to which this aspect is applied.</summary>
		/// <param name="args">Event arguments specifying which method is being executed, which are its arguments, and how should the execution continue after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)"/>.</param>
		public override void OnEntry(MethodExecutionArgs args)
		{
			foreach (IMethodPrecondition validator in this.MethodPreconditions)
				validator.OnMethodPrecondition(this.FriendlyName, args);
			foreach (ParameterMetadata validator in this.ParameterPreconditions)
				validator.Annotation.OnParameterPrecondition(this.FriendlyName, validator, args.Arguments[validator.Index]);
		}

		/// <summary>Method executed <b>after</b> the body of methods to which this aspect is applied, but only when the method successfully returns (i.e. when no exception flies out the method.).</summary>
		/// <param name="args">Event arguments specifying which method is being executed and which are its arguments.</param>
		public override void OnSuccess(MethodExecutionArgs args)
		{
			foreach (IReturnValuePrecondition validator in this.ReturnValuePreconditions)
				validator.OnReturnValuePrecondition(this.FriendlyName, args.ReturnValue);
			foreach (IMethodPostcondition validator in this.MethodPostconditions)
				validator.OnMethodPostcondition(this.FriendlyName, args);
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get a human-readable name representing the target being validated for use in exception messages.</summary>
		/// <param name="method">The method for which to generate a friendly name.</param>
		protected string GetFriendlyName(MethodBase method)
		{
			return String.Format("{0}::{1}", method.DeclaringType.Name, method.Name);
		}

		/// <summary>Get the contract requirements that must be met before a method is executed.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected IMethodPrecondition[] GetMethodPreconditions(MethodBase method, bool inheritContracts = true)
		{
			return this
				.GetCustomAttributes<IMethodPrecondition>(method, this.InheritContract)
				.ToArray();
		}

		/// <summary>Get the contract requirements that must be met after a method is executed.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected IMethodPostcondition[] GetMethodPostconditions(MethodBase method, bool inheritContracts = true)
		{
			return this
				.GetCustomAttributes<IMethodPostcondition>(method, this.InheritContract)
				.ToArray();
		}

		/// <summary>Get the contract requirements for each method parameter or property setter value.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected ParameterMetadata[] GetParameterPreconditions(MethodBase method, bool inheritContracts = true)
		{
			IEnumerable<ParameterMetadata> conditions = (
				from metadata in method.GetParameters().Select((parameter, index) => new { parameter, index })
				from object annotation in this.GetCustomAttributes<IParameterPrecondition>(metadata.parameter, inheritContracts)
				select new ParameterMetadata { Parameter = metadata.parameter, Index = metadata.index, Annotation = annotation as IParameterPrecondition }
			);

			// TODO: if this is a property, check conditions on method

			return conditions.ToArray();
		}

		/// <summary>Get the contract requirements on a method or property return value.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected IReturnValuePrecondition[] GetReturnValuePreconditions(MethodBase method, bool inheritContracts = true)
		{
			if (method is MethodInfo)
			{
				MethodInfo methodInfo = method as MethodInfo;
				return this
					.GetCustomAttributes<IReturnValuePrecondition>(methodInfo.ReturnTypeCustomAttributes, inheritContracts)
					.Union(this.GetCustomAttributes<IReturnValuePrecondition>(method, inheritContracts))
					.ToArray();
			}
			return new IReturnValuePrecondition[0];
		}

		/// <summary>Get the custom attributes of a given type from a provider.</summary>
		/// <typeparam name="T">The type of the custom attributes.</typeparam>
		/// <param name="customAttributeProvider">The object from which to retrieve custom attributes.</param>
		/// <param name="inherit">Whether to inherit attributes from base classes and interfaces.</param>
		protected IEnumerable<T> GetCustomAttributes<T>(ICustomAttributeProvider customAttributeProvider, bool inherit)
		{
			return customAttributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
		}
	}
}
