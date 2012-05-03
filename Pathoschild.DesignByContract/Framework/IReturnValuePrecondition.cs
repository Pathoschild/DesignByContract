using System;

namespace Pathoschild.DesignByContract.Framework
{
	// <summary>Represents a contract annotation for a requirement on a method or property return value.</summary>
	public interface IReturnValuePrecondition
	{
		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		void OnReturnValuePrecondition(string friendlyName, object value);
	}
}