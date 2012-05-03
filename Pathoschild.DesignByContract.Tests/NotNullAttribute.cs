using System;
using NUnit.Framework;

namespace Pathoschild.DesignByContract.Tests
{
	/// <summary>Unit tests for <see cref="NotNullAttribute"/>.</summary>
	[TestFixture]
	public class NotNullAttributeTests
	{
		/*********
		** Unit tests
		*********/
		[Test(Description = "[NotNull] on a method parameter should throw an appropriate exception if its value is null or consists entirely of whitespace.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(ArgumentException))]
		[TestCase(null, ExpectedException = typeof(ArgumentNullException))]
		public string OnParameter(string value)
		{
			return this.MethodWithAnnotatedParameter(value);
		}

		[Test(Description = "[NotNull] on a method return value should throw an appropriate exception if its value is null or consists entirely of whitespace.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(InvalidOperationException))]
		[TestCase(null, ExpectedException = typeof(NullReferenceException))]
		public string OnReturnValue(string value)
		{
			return this.MethodWithAnnotatedReturnValue(value);
		}

		[Test(Description = "[NotNull] on a method should be equivalent to a return value method.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(InvalidOperationException))]
		[TestCase(null, ExpectedException = typeof(NullReferenceException))]
		public string OnMethod(string value)
		{
			return this.MethodWithAnnotation(value);
		}


		/*********
		** Demo methods
		*********/
		/// <summary>An example method with an annotated argument.</summary>
		/// <param name="value">The annotated argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		[DesignedByContract]
		protected string MethodWithAnnotatedParameter([NotNull] string value)
		{
			return value;
		}

		/// <summary>An example method with an annotated return value.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		[DesignedByContract]
		[return: NotNull]
		protected string MethodWithAnnotatedReturnValue(string value)
		{
			return value;
		}

		/// <summary>An example method with a method annotation attribute.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		[DesignedByContract]
		[NotNull]
		protected string MethodWithAnnotation(string value)
		{
			return value;
		}
	}
}
