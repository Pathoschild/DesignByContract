using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value is not <c>null</c> or equal to the default value for its type.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotDefaultAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
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
			if (IsNullOrDefault(value))
				throw new ParameterContractException(parameter, "cannot have the default value");
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (IsNullOrDefault(value))
				throw new ReturnValueContractException(returnValue, "cannot have the default value");
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether a value is <c>null</c> or equal to the default value for its type.</summary>
		/// <param name="value">The value to check.</param>
		private static bool IsNullOrDefault(object value)
		{
			if (value == null)
				return true;

			Type type = value.GetType();
			return type.IsValueType && value.Equals(Activator.CreateInstance(type));
		}
	}
}