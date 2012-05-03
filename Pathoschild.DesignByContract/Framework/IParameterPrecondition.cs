using System;

namespace Pathoschild.DesignByContract.Framework
{
	// <summary>Represents a contract annotation for a requirement on a single method parameter or property setter value.</summary>
	public interface IParameterPrecondition
	{
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the input parameter to check.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		void OnParameterPrecondition(string friendlyName, ParameterMetadata parameter, object value);
	}
}