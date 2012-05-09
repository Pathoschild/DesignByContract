using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value not be <c>null</c>.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotNullAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Public methods
		*********/
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ArgumentNullException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (value == null)
				throw new ArgumentNullException(parameter.Name, parameter.GetMessage("cannot be null"));
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="NullReferenceException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (value == null)
				throw new NullReferenceException(returnValue.GetMessage("cannot be null"));
		}
	}
}
