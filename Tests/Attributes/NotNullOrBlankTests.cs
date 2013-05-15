using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotNullOrBlankAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotNullOrBlankTests
	{
		/*********
		** Unit tests
		*********/
		[ParameterContractTestCase("a valid value", false)]
		[ParameterContractTestCase("   ", true)]
		[ParameterContractTestCase("", true)]
		[ParameterContractTestCase(null, false)]
		public string OnParameter([NotNullOrBlank] string value)
		{
			return value;
		}

		[ReturnValueContractTestCase("a valid value", false)]
		[ReturnValueContractTestCase("   ", true)]
		[ReturnValueContractTestCase("", true)]
		[ReturnValueContractTestCase(null, false)]
		[return: NotNullOrBlank]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}