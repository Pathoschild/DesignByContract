namespace Pathoschild.DesignByContract.Tests.Base
{
	public interface ISword
	{
		/// <summary>An example method with an annotated argument.</summary>
		/// <param name="value">The annotated argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		string MethodWithAnnotatedParameter([NotBlank] string value);

		/// <summary>An example method with an annotated return value.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		string MethodWithAnnotatedReturnValue(string value);

		/// <summary>An example method with a method annotation attribute.</summary>
		/// <param name="value">The argument value.</param>
		/// <returns>Returns the <paramref name="value"/>.</returns>
		string MethodWithAnnotation(string value);
	}
}
