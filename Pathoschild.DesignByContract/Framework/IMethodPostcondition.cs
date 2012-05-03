using System;
using PostSharp.Aspects;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a contract annotation for a requirement that must be met after a method is executed.</summary>
	public interface IMethodPostcondition
	{
		/// <summary>Validate the requirement for method completion.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="method">The method whose precondition to validate.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		void OnMethodPostcondition(string friendlyName, MethodExecutionArgs method);
	}
}