using System;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a contract annotation on a method return value.</summary>
	public interface IReturnValueContractAnnotation
	{
		/// <summary>Assert that the method return value respects the contract condition represented by this annotation.</summary>
		/// <param name="friendlyName">A human-readable name representing the method returning the value for use in exception messages.</param>
		/// <param name="value">The return value to check.</param>
		/// <exception cref="Exception">The method return value failed the contract condition.</exception>
		void CheckReturnValue(string friendlyName, object value);
	}
}