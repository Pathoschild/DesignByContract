using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotNullOrWhiteSpaceAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
    public class NotNullOrWhiteSpaceTests
	{
		/*********
		** Unit tests
		*********/
		[ParameterContractTestCase("a valid value", false)]
		[ParameterContractTestCase("   ", true)]
		[ParameterContractTestCase("", true)]
		[ParameterContractTestCase(null, false)]
        public string OnParameter([NotNullOrWhiteSpace] string value)
		{
			return value;
		}

		[ReturnValueContractTestCase("a valid value", false)]
		[ReturnValueContractTestCase("   ", true)]
		[ReturnValueContractTestCase("", true)]
		[ReturnValueContractTestCase(null, false)]
        [return: NotNullOrWhiteSpace]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
