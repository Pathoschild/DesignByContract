using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Shorthand;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes.Shorthand
{
	/// <summary>Unit tests for <see cref="NotNullOrBlankAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class NotNullOrBlankTests
	{
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(ArgumentException))]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null, typeof(ArgumentNullException))]
		public string OnParameter([NotNullOrBlank] string value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null, typeof(NullReferenceException))]
		[return: NotNullOrBlank]
		public string OnReturnValue(string value)
		{
			return value;
		}
	}
}
