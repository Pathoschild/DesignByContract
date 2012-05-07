using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotBlankAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotBlankTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(ArgumentException))]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null)]
		public string OnParameter([NotBlank] string value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		[return: NotBlank]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
