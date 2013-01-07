using NUnit.Framework;

namespace Pathoschild.DesignByContract.Tests
{
	/// <summary>Marks a parameterized test case for a return value contract whose first argument is returned.</summary>
	public class ParameterContractTestCaseAttribute : TestCaseAttribute
	{
		/// <summary>Construct a unit test case.</summary>
		/// <param name="value">The value passed to the unit test parameter and expected in return.</param>
		/// <param name="violatesContract">Whether this value violates the parameter contract being tests.</param>
		public ParameterContractTestCaseAttribute(object value, bool violatesContract)
			: base(value)
		{
			this.Result = value;
			if (violatesContract)
				this.ExpectedException = typeof(ParameterContractException);
		}
	}
}