using System;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a contract annotation on a method parameter.</summary>
	public interface IParameterContractAnnotation
	{
		/// <summary>Assert that the method parameter respects the contract condition represented by this annotation.</summary>
		/// <param name="friendlyName">A human-readable name representing the target being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the parameter to check.</param>
		/// <param name="value">The value of the parameter to check.</param>
		/// <exception cref="Exception">The method parameter failed the contract condition.</exception>
		void CheckParameter(string friendlyName, ParameterMetadata parameter, object value);
	}
}