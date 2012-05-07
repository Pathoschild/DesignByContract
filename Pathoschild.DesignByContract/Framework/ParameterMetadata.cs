using System;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a parameter annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ParameterMetadata"/> representation.</remarks>
	[Serializable]
	public struct ParameterMetadata
	{
		/*********
		** Accessors
		*********/
		/// <summary>The annotated method parameter.</summary>
		public ParameterInfo Parameter { get; set; }

		/// <summary>The zero-based position of the parameter in the list of method parameters.</summary>
		public int Index { get; set; }

		/// <summary>The contract annotation applied to the parameter.</summary>
		public IParameterPrecondition Annotation { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="parameter">The annotated method parameter.</param>
		/// <param name="index">The zero-based position of the parameter in the list of method parameters.</param>
		/// <param name="annotation">The contract annotation applied to the parameter.</param>
		public ParameterMetadata(ParameterInfo parameter, int index, IParameterPrecondition annotation)
			: this()
		{
			this.Parameter = parameter;
			this.Index = index;
			this.Annotation = annotation;
		}
	}
}