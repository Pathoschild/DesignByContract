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
		[ParameterContractTestCase("a valid value", false)]
		[ParameterContractTestCase(typeof(int), false)]
		[ParameterContractTestCase(42, true)]
		[ParameterContractTestCase(null, false)]
		public object OnParameter([HasType(typeof(string), typeof(IReflect))] object value)
		{
			return value;
		}

		[ReturnValueContractTestCase("a valid value", false)]
		[ReturnValueContractTestCase(typeof(int), false)]
		[ReturnValueContractTestCase(42, true)]
		[ReturnValueContractTestCase(null, false)]
		[return: HasType(typeof(string), typeof(IReflect))]
		public object OnReturnValue(object value)
		{
			return value;
		}
	}
}