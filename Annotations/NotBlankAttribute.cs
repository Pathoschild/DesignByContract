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
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ParameterContractException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (this.IsWhitespace(value))
				throw new ParameterContractException(parameter, "cannot be blank or consist entirely of whitespace");
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (this.IsWhitespace(value))
				throw new ReturnValueContractException(returnValue, "cannot be blank or consist entirely of whitespace");
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
