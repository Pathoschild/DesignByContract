﻿using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotEmptyAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotEmptyTests
	{
		/*********
		** Unit tests
		*********/
		[ParameterContractTestCase("a valid value", false)]
		[ParameterContractTestCase("   ", false)]
		[ParameterContractTestCase("", true)]
		[ParameterContractTestCase(null, false)]
		public string OnParameter([NotEmpty] string value)
		{
			return value;
		}

		[ReturnValueContractTestCase("a valid value", false)]
		[ReturnValueContractTestCase("   ", false)]
		[ReturnValueContractTestCase("", true)]
		[ReturnValueContractTestCase(null, false)]
		[return: NotEmpty]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}