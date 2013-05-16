using System;
using System.Collections;
using System.Linq;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Constraints;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value not be an empty enumeration.</summary>
	[Serializable]
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[RequiresType(typeof(string), typeof(IEnumerable))]
	public class NotEmptyAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Properties
		*********/
		/// <summary>Whether to raise a validation error on <c>null</c> values.</summary>
		protected bool RaiseErrorOnNull = false;


		/*********
		** Public methods
		*********/
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ParameterContractException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (this.IsNull(value))
				throw new ParameterContractException(parameter, "cannot be null");
			if (this.IsEmpty(value))
				throw new ParameterContractException(parameter, "cannot be an empty enumeration");
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (this.IsNull(value))
				throw new ReturnValueContractException(returnValue, "cannot be null");
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
			if (value is string)
				return String.IsNullOrEmpty(value as string);
			return value is IEnumerable && !(value as IEnumerable).Cast<object>().Any();
		}

		/// <summary>Get whether the value is <c>null</c> and <see cref="RaiseErrorOnNull"/> is <c>true</c>.</summary>
		/// <param name="value">The value to check.</param>
		protected bool IsNull(object value)
		{
			return this.RaiseErrorOnNull && value == null;
		}
	}
}