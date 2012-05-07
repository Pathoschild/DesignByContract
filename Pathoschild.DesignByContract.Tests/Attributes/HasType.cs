using System;
using System.Reflection;
using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="HasTypeAttribute"/>.</summary>
	/// <remarks>These tests assume that the <see cref="DesignedByContractTests"/> pass.</remarks>
	[TestFixture]
	[DesignedByContract]
	public class HasTypeTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase(typeof(int))]
		[TestReturnCase(42, typeof(ArgumentException))]
		[TestReturnCase(null)]
		public object OnParameter([HasType(typeof(string), typeof(IReflect))] object value)
		{
			return value;
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase(typeof(int))]
		[TestReturnCase(42, typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		[return: HasType(typeof(string), typeof(IReflect))]
		public object OnReturnValue(object value)
		{
			return value;
		}
	}
}
