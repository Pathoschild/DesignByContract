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
		[Test(Description = "[NotNull] on a method argument should throw an appropriate exception if its value is null or consists entirely of whitespace.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(ArgumentException))]
		[TestCase(null, ExpectedException = typeof(ArgumentNullException))]
		public string ArgumentAnnotation(string value)
		{
			return this.MethodWithAnnotatedArgument(value);
		}

		[Test(Description = "[NotNull] on a method return value should throw an appropriate exception if its value is null or consists entirely of whitespace.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(InvalidOperationException))]
		[TestCase(null, ExpectedException = typeof(NullReferenceException))]
		public string ReturnValueAnnotation(string value)
		{
			return this.MethodWithAnnotatedReturnValue(value);
		}

		[Test(Description = "[NotNull] on a method itself should throw an appropriate exception if its return value is null or consists entirely of whitespace.")]
		[TestCase("a valid value", Result = "a valid value")]
		[TestCase("", ExpectedException = typeof(InvalidOperationException))]
		[TestCase(null, ExpectedException = typeof(NullReferenceException))]
		public string MethodAnnotation(string value)
		{
			return this.MethodWithAnnotatedAttribute(value);
		}


		/*********
		** Demo methods
		*********/
		/// <summary>An example method with an annotated argument.</summary>
		/// <param name="value">The annotated argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		[DesignedByContract, NotNull]
		protected string MethodWithAnnotatedArgument([NotNull] string value)
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
		protected string MethodWithAnnotatedAttribute(string value)
		{
			return value;
		}
	}
}
