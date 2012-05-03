using System;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a parameter annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ParameterMetadata"/> representation.</remarks>
	[Serializable]
	public struct ParameterMetadata
	{
		/// <summary>The annotated method parameter.</summary>
		public ParameterInfo Parameter { get; set; }

		/// <summary>The zero-based position of the parameter in the list of method parameters.</summary>
		public int Index { get; set; }

		/// <summary>The contract annotation applied to the parameter.</summary>
		public IParameterContractAnnotation Annotation { get; set; }
	}
}