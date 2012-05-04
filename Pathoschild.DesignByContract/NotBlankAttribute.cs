using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value not be a string that consists entirely of whitespace.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotBlankAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
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
			if (this.IsWhitespace(value))
				throw new ArgumentException(String.Format("The value cannot be blank or consist entirely of whitespace for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName), parameter.Parameter.Name);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(string friendlyName, object value)
		{
			if (this.IsWhitespace(value))
				throw new InvalidOperationException(String.Format("The return value cannot be blank or consist entirely of whitespace for method '{0}'.", friendlyName));
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether the value is a string which is empty or consists entirely of whitespace.</summary>
		/// <param name="value">The value to check.</param>
		protected bool IsWhitespace(object value)
		{
			return value != null && value is string && String.IsNullOrWhiteSpace(value as string);
		}
	}
}
