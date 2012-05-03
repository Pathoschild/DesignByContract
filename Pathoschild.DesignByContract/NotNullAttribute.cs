using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract annotation which ensures that a method parameter or return value is not <c>null</c> and (if a string) does not consist entirely of whitespace.</summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Property)]
	[Serializable]
	public class NotNullAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Public methods
		*********/
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the input parameter to check.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(string friendlyName, ParameterMetadata parameter, object value)
		{
			if (value == null)
				throw new ArgumentNullException(parameter.Parameter.Name, String.Format("The value cannot be null for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName));

			if (value is string && String.IsNullOrWhiteSpace(value as string))
				throw new ArgumentException(String.Format("The value cannot be blank or consist entirely of whitespace for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName), parameter.Parameter.Name);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(string friendlyName, object value)
		{
			if (value == null)
				throw new NullReferenceException(String.Format("The return value cannot be null for method '{0}'.", friendlyName));

			if (value is string && String.IsNullOrWhiteSpace(value as string))
				throw new InvalidOperationException(String.Format("The return value cannot be blank or consist entirely of whitespace for method '{0}'.", friendlyName));

		}
	}
}
