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
		/// <exception cref="ArgumentException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (this.IsEmpty(value))
				throw new ArgumentException(parameter.GetMessage("cannot be an empty enumeration"), parameter.Name);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (this.IsEmpty(value))
				throw new InvalidOperationException(returnValue.GetMessage("cannot be an empty enumeration"));
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
