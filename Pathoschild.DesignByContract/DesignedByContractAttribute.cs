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
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor)]
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

		/// <summary>The cached contract metadata for the method's parameters.</summary>
		public ParameterMetadata[] ParameterAnnotations { get; set; }

		/// <summary>The cached contract metadata for the method's return value.</summary>
		public IReturnValueContractAnnotation[] ReturnValueAnnotations { get; set; }


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
			this.FriendlyName = this.GetFriendlyName(method);
			this.ParameterAnnotations = this.GetParameterAnnotations(method, this.InheritContract);
			this.ReturnValueAnnotations = this.GetReturnValueAnnotations(method, this.InheritContract);

			base.CompileTimeInitialize(method, aspectInfo);
		}

		/// <summary>Method executed <b>before</b> the body of methods to which this aspect is applied.</summary>
		/// <param name="args">Event arguments specifying which method is being executed, which are its arguments, and how should the execution continue after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)"/>.</param>
		public override void OnEntry(MethodExecutionArgs args)
		{
			foreach (var validator in this.ParameterAnnotations)
				validator.Annotation.CheckParameter(this.FriendlyName, validator, args.Arguments[validator.Index]);
			base.OnEntry(args);
		}

		/// <summary>Method executed <b>after</b> the body of methods to which this aspect is applied, but only when the method successfully returns (i.e. when no exception flies out the method.).</summary>
		/// <param name="args">Event arguments specifying which method is being executed and which are its arguments.</param>
		public override void OnSuccess(MethodExecutionArgs args)
		{
			foreach (var validator in this.ReturnValueAnnotations)
				validator.CheckReturnValue(this.FriendlyName, args.ReturnValue);
			base.OnSuccess(args);
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

		/// <summary>Get the contract metadata for a a method's parameters.</summary>
		/// <param name="method">The method whose parameters to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected ParameterMetadata[] GetParameterAnnotations(MethodBase method, bool inheritContracts = true)
		{
			return (
				from metadata in method.GetParameters().Select((parameter, index) => new { parameter, index })
				from object annotation in this.GetCustomAttributes<IParameterContractAnnotation>(metadata.parameter, inheritContracts)
				select new ParameterMetadata { Parameter = metadata.parameter, Index = metadata.index, Annotation = annotation as IParameterContractAnnotation }
			).ToArray();
		}

		/// <summary>Get the contract metadata for a a method's return type.</summary>
		/// <param name="method">The method whose return type annotations to analyze.</param>
		/// <param name="inheritContracts">Whether to inherit contracts from base classes and interfaces.</param>
		protected IReturnValueContractAnnotation[] GetReturnValueAnnotations(MethodBase method, bool inheritContracts = true)
		{
			if (method is MethodInfo)
			{
				MethodInfo methodInfo = method as MethodInfo;
				return this
					.GetCustomAttributes<IReturnValueContractAnnotation>(methodInfo.ReturnTypeCustomAttributes, inheritContracts)
					.Union(
						this.GetCustomAttributes<IReturnValueContractAnnotation>(method, inheritContracts)
					)
					.ToArray();
			}
			return new IReturnValueContractAnnotation[0];
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
