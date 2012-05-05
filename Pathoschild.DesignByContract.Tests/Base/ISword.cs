namespace Pathoschild.DesignByContract.Tests.Base
{
	/// <summary>Represents example annotated code.</summary>
	/// <typeparam name="T">The type of argument and return values.</typeparam>
	public interface ISword<T>
	{
		/// <summary>An example method with an annotated argument.</summary>
		/// <param name="value">The annotated argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		T MethodWithAnnotatedParameter(T value);

		/// <summary>An example method with an annotated return value.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		T MethodWithAnnotatedReturnValue(T value);

		/// <summary>An example method with a method annotation attribute.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		T MethodWithAnnotation(T value);
	}

	/// <summary>Represents example annotated code.</summary>
	public interface ISword : ISword<string> { }
}
