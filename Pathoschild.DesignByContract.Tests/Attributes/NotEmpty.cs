using System;
using NUnit.Framework;
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
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null)]
		public string OnParameter([NotEmpty] string value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		[return: NotEmpty]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
