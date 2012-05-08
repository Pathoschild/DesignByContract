using System;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Extension methods useful to contract annotations.</summary>
	public static class AnnotationExtensions
	{
		/// <summary>Get an exception message for a contract violation on this parameter.</summary>
		/// <param name="parameter">The validated parameter.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public static string GetMessage(this ParameterMetadata parameter, string error)
		{
			return String.Format(parameter.MessageFormat, error);
		}

		/// <summary>Get an exception message for a contract violation on this return value.</summary>
		/// <param name="returnValue">The validated return value.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public static string GetMessage(this ReturnValueMetadata returnValue, string error)
		{
			return String.Format(returnValue.MessageFormat, error);
		}
	}
}
