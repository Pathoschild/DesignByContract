using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotNullAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotNullTests
	{
		[ParameterContractTestCase("a valid value", false)]
		[ParameterContractTestCase("   ", false)]
		[ParameterContractTestCase("", false)]
		[ParameterContractTestCase(null, true)]
		public string OnParameter([NotNull] string value)
		{
			return value;
		}

		[ReturnValueContractTestCase("a valid value", false)]
		[ReturnValueContractTestCase("   ", false)]
		[ReturnValueContractTestCase("", false)]
		[ReturnValueContractTestCase(null, true)]
		[return: NotNull]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}