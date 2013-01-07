using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a parameter annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ParameterMetadata"/> representation.</remarks>
	[Serializable, DataContract]
	public struct ParameterMetadata
	{
		/*********
		** Accessors
		*********/
		/// <summary>The name of the type.</summary>
		[DataMember]
		public string TypeName { get; set; }

		/// <summary>The name of the method.</summary>
		[DataMember]
		public string MethodName { get; set; }

		/// <summary>The name of the annotated method parameter.</summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>The zero-based position of the parameter in the list of method parameters.</summary>
		[DataMember]
		public int Position { get; set; }

		/// <summary>The contract annotation applied to the parameter.</summary>
		[DataMember]
		public IParameterPrecondition Annotation { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="parameter">The annotated method parameter.</param>
		/// <param name="annotation">The contract annotation applied to the parameter.</param>
		/// <param name="methodName">The method name if different from the underlying name. (This is primarily intended for cases where <see cref="MethodBase.IsSpecialName"/> is <c>true</c>).</param>
		public ParameterMetadata(ParameterInfo parameter, IParameterPrecondition annotation, string methodName = null)
			: this()
		{
			this.TypeName = parameter.Member.DeclaringType.Name;
			this.MethodName = methodName ?? parameter.Member.Name;
			this.Name = parameter.Name;
			this.Position = parameter.Position;
			this.Annotation = annotation;
		}
	}
}