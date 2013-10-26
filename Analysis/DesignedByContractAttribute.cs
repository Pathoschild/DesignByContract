using System;
using System.Reflection;
using System.Runtime.Serialization;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Analysis;
using PostSharp.Aspects;

namespace Pathoschild.DesignByContract
{
	/// <summary>Indicates that invocations of the target methods should be validated to ensure that their preconditions or postconditions are respected.</summary>
	/// <remarks>This aspect analyzes annotated code at compile time, serializes the reflection metadata into its cache, and intercepts method calls at runtime to check conditions.</remarks>
	[AttributeUsage(AttributeTargets.All)]
	[Serializable, DataContract]
	public class DesignedByContractAttribute : OnMethodBoundaryAspect
	{
		/*********
		** Accessors
		*********/
		/// <summary>Whether to inherit contract annotations defined on a base class or interface.</summary>
		[DataMember]
		public bool InheritContracts { get; set; }

		/// <summary>Contains contract metadata about an annotated method or property.</summary>
		[DataMember]
		public MethodAnalysis Metadata;

		/// <summary>Reflects methods and properties for contract analysis.</summary>
		[NonSerialized, IgnoreDataMember]
		protected IMethodAnalyzer Analyzer;


		/*********
		** Public methods
		*********/
		/// <summary>Construct an annotation which indicates that invocations of the target methods should be validated to ensure that their preconditions or postconditions are respected.</summary>
		public DesignedByContractAttribute()
			: this(MethodAnalyzer.Instance) { }

		/// <summary>Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked before any other build-time method.</summary>
		/// <param name="method">Method to which the current aspect is applied</param>
		/// <param name="aspectInfo">Reserved for future usage.</param>
		/// <remarks>This implementation analyzes the annotated method and prepares the conditions for runtime verification.</remarks>
		public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
		{
			try
			{
				this.Metadata = this.Analyzer.AnalyzeMethod(method, this.InheritContracts);
			}
			catch (Exception exception)
			{
				throw new InvalidOperationException(String.Format("Method analysis failed for method {0}::{1}.", method.DeclaringType.Name, method.Name), exception);
			}
		}

		/// <summary>Method invoked at build time to determine whether code needs to be injected for this method.</summary>
		/// <param name="method">Method to which the current aspect is applied</param>
		public override bool CompileTimeValidate(MethodBase method)
		{
			return this.Metadata.HasContract;
		}

		/// <summary>Method executed <b>before</b> the body of methods to which this aspect is applied.</summary>
		/// <param name="args">Event arguments specifying which method is being executed, which are its arguments, and how should the execution continue after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)"/>.</param>
		public sealed override void OnEntry(MethodExecutionArgs args)
		{
			foreach (ParameterMetadata metadata in this.Metadata.ParameterPreconditions)
				metadata.Annotation.OnParameterPrecondition(metadata, args.Arguments[metadata.Position]);
		}

		/// <summary>Method executed <b>after</b> the body of methods to which this aspect is applied, but only when the method successfully returns (i.e. when no exception flies out the method.).</summary>
		/// <param name="args">Event arguments specifying which method is being executed and which are its arguments.</param>
		public sealed override void OnSuccess(MethodExecutionArgs args)
		{
			foreach (ReturnValueMetadata metadata in this.Metadata.ReturnValuePreconditions)
				metadata.Annotation.OnReturnValuePrecondition(metadata, args.ReturnValue);
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="analyzer">Reflects methods and properties for contract analysis.</param>
		/// <param name="inheritContract">Whether to inherit the contract annotations defined on a base class or interface.</param>
		protected DesignedByContractAttribute(IMethodAnalyzer analyzer, bool inheritContract = true)
		{
			if (analyzer == null)
				throw new ArgumentNullException("analyzer");

			this.Analyzer = analyzer;
			this.InheritContracts = inheritContract;
		}
	}
}