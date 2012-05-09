using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract.Shorthand
{
	/// <summary>A contract precondition that a value not be <c>null</c> nor a string that consists entirely of whitespace.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotNullOrBlankAttribute : NotBlankAttribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Public methods
		*********/
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ParameterContractException">The contract requirement was not met.</exception>
		public new void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (value == null)
				throw new ParameterContractException(parameter, "cannot be null");
			base.OnParameterPrecondition(parameter, value);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		public new void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (value == null)
				throw new ReturnValueContractException(returnValue, "cannot be null");
			base.OnReturnValuePrecondition(returnValue, value);
		}
	}
}
