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
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the input parameter to check.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(string friendlyName, ParameterMetadata parameter, object value)
		{
			if (this.IsEmpty(value))
				throw new ArgumentException(parameter.Parameter.Name, String.Format("The enumeration cannot be empty for parameter '{0}' of method {1}.", parameter.Parameter.Name, friendlyName));
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(string friendlyName, object value)
		{
			if (this.IsEmpty(value))
				throw new InvalidOperationException(String.Format("The return value cannot be an empty enumeration for method '{0}'.", friendlyName));
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
