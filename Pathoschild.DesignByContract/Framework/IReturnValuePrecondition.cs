using System;

namespace Pathoschild.DesignByContract.Framework
{
	// <summary>Represents a contract annotation for a requirement on a method or property return value.</summary>
	public interface IReturnValuePrecondition
	{
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		


		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value);
	}
}