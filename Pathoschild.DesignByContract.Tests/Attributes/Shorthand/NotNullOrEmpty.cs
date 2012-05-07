using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Shorthand;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes.Shorthand
{
	/// <summary>Unit tests for <see cref="NotNullOrEmptyAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotNullOrEmptyTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null, typeof(ArgumentNullException))]
		public string OnParameter([NotNullOrEmpty] string value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ")]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null, typeof(NullReferenceException))]
		[return: NotNullOrEmpty]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
