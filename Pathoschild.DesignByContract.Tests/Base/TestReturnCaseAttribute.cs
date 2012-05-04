using System;
using NUnit.Framework;

namespace Pathoschild.DesignByContract.Tests.Base
{
	/// <summary>Marks parameterized test cases whose first argument is returned.</summary>
	public class TestReturnCaseAttribute : TestCaseAttribute
	{
		/// <summary>Construct a unit test case.</summary>
		/// <param name="value">The value passed to the unit test parameter and expected in return.</param>
		public TestReturnCaseAttribute(object value)
			: base(value)
		{
			this.Result = value;
		}

		/// <summary>Construct a unit test case.</summary>
		/// <param name="value">The value passed to the unit test parameter and expected in return.</param>
		/// <param name="expectedException">The exception message that should be thrown.</param>
		public TestReturnCaseAttribute(object value, Type expectedException)
			: this(value)
		{
			this.ExpectedException = expectedException;
		}
	}
}
