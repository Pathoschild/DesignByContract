using System;
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
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("")]
		[TestReturnCase(null, typeof(ArgumentNullException))]
		public string OnParameter([NotNull] string value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("")]
		[TestReturnCase(null, typeof(NullReferenceException))]
		[return: NotNull]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
