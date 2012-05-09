using System;
using Pathoschild.DesignByContract.Exceptions;

namespace Pathoschild.DesignByContract.Framework
{
	// <summary>Represents a contract annotation for a requirement on a method or property return value.</summary>
	public interface IReturnValuePrecondition
	{
		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value);
	}
}