using System;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a return value annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ReturnValueMetadata"/> representation.</remarks>
	[Serializable]
	public struct ReturnValueMetadata
	{
		/*********
		** Accessors
		*********/
		/// <summary>The suggested exception message format, where {0} is the message.</summary>
		public string MessageFormat { get; set; }

		/// <summary>The contract annotation applied to the return value.</summary>
		public IReturnValuePrecondition Annotation { get; set; }

		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="messageFormat">The suggested exception message format, where {0} is the message.</param>
		/// <param name="annotation">The contract annotation applied to the return value.</param>
		public ReturnValueMetadata(string messageFormat, IReturnValuePrecondition annotation)
			: this()
		{
			this.MessageFormat = messageFormat;
			this.Annotation = annotation;
		}
	}
}