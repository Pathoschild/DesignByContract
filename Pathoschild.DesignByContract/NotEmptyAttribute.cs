using System;
using System.Collections;
using System.Linq;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value not be an empty enumeration.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotEmptyAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
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
			if (this.IsEmpty(value))
				throw new ParameterContractException(parameter, "cannot be an empty enumeration");
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (this.IsEmpty(value))
				throw new ReturnValueContractException(returnValue, "cannot be an empty enumeration");
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether the value is an empty enumeration.</summary>
		/// <param name="value">The value to check.</param>
		protected bool IsEmpty(object value)
		{
			return value != null && value is IEnumerable && !(value as IEnumerable).Cast<object>().Any();
		}
	}
}
