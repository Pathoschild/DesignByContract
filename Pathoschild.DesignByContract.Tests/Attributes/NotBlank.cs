using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Base;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="NotBlankAttribute"/>.</summary>
	[TestFixture]
	public class NotBlankTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(ArgumentException))]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null)]
		public string OnParameter(string value)
		{
			return new Sword().MethodWithAnnotatedParameter(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		public string OnReturnValue(string value)
		{
			return new Sword().MethodWithAnnotatedReturnValue(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		public string OnMethod(string value)
		{
			return new Sword().MethodWithAnnotation(value);
		}


		/*********
		** Objects
		*********/
		[DesignedByContract]
		protected class Sword : ISword
		{
			/// <summary>An example method with an annotated argument.</summary>
			/// <param name="value">The annotated argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			public string MethodWithAnnotatedParameter([NotBlank] string value)
			{
				return value;
			}

			/// <summary>An example method with an annotated return value.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[return: NotBlank]
			public string MethodWithAnnotatedReturnValue(string value)
			{
				return value;
			}

			/// <summary>An example method with a method annotation attribute.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[NotBlank]
			public string MethodWithAnnotation(string value)
			{
				return value;
			}
		}
	}
}
