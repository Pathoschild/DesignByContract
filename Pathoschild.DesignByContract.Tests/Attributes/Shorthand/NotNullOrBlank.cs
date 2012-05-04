using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Shorthand;
using Pathoschild.DesignByContract.Tests.Base;

namespace Pathoschild.DesignByContract.Tests.Attributes.Shorthand
{
	/// <summary>Unit tests for <see cref="NotNullOrBlankAttribute"/>.</summary>
	[TestFixture]
	public class NotNullOrBlankTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(ArgumentException))]
		[TestReturnCase("", typeof(ArgumentException))]
		[TestReturnCase(null, typeof(ArgumentNullException))]
		public string OnParameter(string value)
		{
			return new Sword().MethodWithAnnotatedParameter(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null, typeof(NullReferenceException))]
		public string OnReturnValue(string value)
		{
			return new Sword().MethodWithAnnotatedReturnValue(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase("   ", typeof(InvalidOperationException))]
		[TestReturnCase("", typeof(InvalidOperationException))]
		[TestReturnCase(null, typeof(NullReferenceException))]
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
			public string MethodWithAnnotatedParameter([NotNullOrBlank] string value)
			{
				return value;
			}

			/// <summary>An example method with an annotated return value.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[return: NotNullOrBlank]
			public string MethodWithAnnotatedReturnValue(string value)
			{
				return value;
			}

			/// <summary>An example method with a method annotation attribute.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[NotNullOrBlank]
			public string MethodWithAnnotation(string value)
			{
				return value;
			}
		}
	}
}
