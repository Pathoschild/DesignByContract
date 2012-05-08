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
		/// <summary>The name of the annotated method parameter.</summary>
		public string ParameterName { get; set; }

		/// <summary>The zero-based position of the parameter in the list of method parameters.</summary>
		public int Index { get; set; }

		/// <summary>The contract annotation applied to the parameter.</summary>
		public IParameterPrecondition Annotation { get; set; }

		/// <summary>The suggested exception message format, where {0} is the message.</summary>
		public string MessageFormat { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="parameter">The annotated method parameter.</param>
		/// <param name="index">The zero-based position of the parameter in the list of method parameters.</param>
		/// <param name="annotation">The contract annotation applied to the parameter.</param>
		/// <param name="messageFormat">The suggested exception message format, where {0} is the message.</param>
		public ParameterMetadata(ParameterInfo parameter, int index, IParameterPrecondition annotation, string messageFormat)
			: this()
		{
			this.ParameterName = parameter.Name;
			this.Index = index;
			this.Annotation = annotation;
			this.MessageFormat = messageFormat;
		}
	}
}