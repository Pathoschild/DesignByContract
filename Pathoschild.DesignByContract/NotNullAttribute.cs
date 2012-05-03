using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract annotation which ensures that the parameter is not <c>null</c> and (if a string) does not consist entirely of whitespace.</summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	[Serializable]
	public class NotNullAttribute : Attribute, IParameterContractAnnotation, IReturnValueContractAnnotation
	{
		/*********
		** Public methods
		*********/
		/// <summary>Assert that the method parameter respects the contract condition represented by this annotation.</summary>
		/// <param name="friendlyName">A human-readable name representing the target being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the parameter to check.</param>
		/// <param name="value">The value of the parameter to check.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The <paramref name="value"/> is blank or consists entirely of whitespace.</exception>
		public void CheckParameter(string friendlyName, ParameterMetadata parameter, object value)
		{
			if (value == null)
				throw new ArgumentNullException(parameter.Parameter.Name, String.Format("The value cannot be null for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName));

			if (value is string && String.IsNullOrWhiteSpace(value as string))
				throw new ArgumentException(String.Format("The value cannot be blank or consist entirely of whitespace for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName), parameter.Parameter.Name);
		}

		/// <summary>Assert that the method return value respects the contract condition represented by this annotation.</summary>
		/// <param name="friendlyName">A human-readable name representing the method returning the value for use in exception messages.</param>
		/// <param name="value">The return value to check.</param>
		/// <exception cref="NullReferenceException">The <paramref name="value"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The <paramref name="value"/> is blank or consists entirely of whitespace.</exception>
		public void CheckReturnValue(string friendlyName, object value)
		{
			if(value == null)
				throw new NullReferenceException(String.Format("The return value cannot be null for method '{0}'.", friendlyName));

			if(value is string && String.IsNullOrWhiteSpace(value as string))
				throw new InvalidOperationException(String.Format("The return value cannot be blank or consist entirely of whitespace for method '{0}'.", friendlyName));
		}
	}
}
