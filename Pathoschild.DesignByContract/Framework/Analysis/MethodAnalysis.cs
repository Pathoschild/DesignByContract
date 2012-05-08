using System;
using System.Linq;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
	/// <summary>Contains contract metadata about an annotated method or property.</summary>
	[Serializable]
	public struct MethodAnalysis
	{
		/// <summary>A human-readable name representing the method or property being validated for use in exception messages.</summary>
		/// <example>For example, the friendly name for a method named "Hit" on a class named "Sword" might be "Sword::Hit".</example>
		public string FriendlyName { get; set; }

		/// <summary>The contract requirements for each method parameter or property setter value.</summary>
		public ParameterMetadata[] ParameterPreconditions { get; set; }

		/// <summary>The contract requirements on a method or property return value.</summary>
		public ReturnValueMetadata[] ReturnValuePreconditions { get; set; }

		/// <summary>Whether the method has contract annotations to enforce.</summary>
		public bool HasContract
		{
			get { return this.ParameterPreconditions.Any() || this.ReturnValuePreconditions.Any(); }
		}
	}
}
