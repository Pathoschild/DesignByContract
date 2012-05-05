using System;
using System.Reflection;
using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Base;

namespace Pathoschild.DesignByContract.Tests.Attributes
{
	/// <summary>Unit tests for <see cref="HasTypeAttribute"/>.</summary>
	[TestFixture]
	public class HasTypeTests
	{
		/*********
		** Unit tests
		*********/
		[TestReturnCase("a valid value")]
		[TestReturnCase(typeof(int))]
		[TestReturnCase(42, typeof(ArgumentException))]
		[TestReturnCase(null)]
		public object OnParameter(object value)
		{
			return new Sword().MethodWithAnnotatedParameter(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase(typeof(int))]
		[TestReturnCase(42, typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		public object OnReturnValue(object value)
		{
			return new Sword().MethodWithAnnotatedReturnValue(value);
		}

		[TestReturnCase("a valid value")]
		[TestReturnCase(typeof(int))]
		[TestReturnCase(42, typeof(InvalidOperationException))]
		[TestReturnCase(null)]
		public object OnMethod(object value)
		{
			return new Sword().MethodWithAnnotation(value);
		}


		/*********
		** Objects
		*********/
		/// <summary>Example annotated code.</summary>
		/// <remarks>The values must implement either <see cref="string"/> or <see cref="IReflect"/> (which is implemented by <see cref="Type"/>).</remarks>
		[DesignedByContract]
		protected class Sword : ISword<object>
		{
			/// <summary>An example method with an annotated argument.</summary>
			/// <param name="value">The annotated argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			public object MethodWithAnnotatedParameter([HasType(typeof(string), typeof(IReflect))] object value)
			{
				return value;
			}

			/// <summary>An example method with an annotated return value.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[return: HasType(typeof(string), typeof(IReflect))]
			public object MethodWithAnnotatedReturnValue(object value)
			{
				return value;
			}

			/// <summary>An example method with a method annotation attribute.</summary>
			/// <param name="value">The argument value.</param>
			/// <returns>Returns the <paramref name="value"/>.</returns>
			[HasType(typeof(string), typeof(IReflect))]
			public object MethodWithAnnotation(object value)
			{
				return value;
			}
		}
	}
}
