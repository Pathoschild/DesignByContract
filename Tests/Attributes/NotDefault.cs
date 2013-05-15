using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotNullAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotDefaultTests
	{
		[ParameterContractTestCase(5, false)]
		[ParameterContractTestCase(0, true)]
		public int OnParameter([NotDefault] int value)
		{
			return value;
		}

		[ReturnValueContractTestCase(5, false)]
		[ReturnValueContractTestCase(0, true)]
		[return: NotDefault]
		public int OnReturnValue(int value)
		{
			return value;
		}

		[ParameterContractTestCase(5, false)]
		[ParameterContractTestCase(0, true)]
		[ParameterContractTestCase(null, true)]
		public int? OnNullableParameter([NotDefault] int? value)
		{
			return value;
		}

		[ReturnValueContractTestCase(5, false)]
		[ReturnValueContractTestCase(0, true)]
		[ReturnValueContractTestCase(null, true)]
		[return: NotDefault]
		public int? OnNullableReturnValue(int? value)
		{
			return value;
		}
	}
}