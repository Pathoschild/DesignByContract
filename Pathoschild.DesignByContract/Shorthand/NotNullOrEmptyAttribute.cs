using System;
using System.Collections;
using System.Linq;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract.Shorthand
{
	/// <summary>A contract precondition that a value not be <c>null</c> nor an empty enumeration.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class NotNullOrEmptyAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Properties
		*********/
		/// <summary>A contract precondition that a value not be <c>null</c>.</summary>
		protected NotNullAttribute NotNullAttribute = new NotNullAttribute();

		/// <summary>A contract precondition that a value not be a string that consists entirely of whitespace.</summary>
		protected NotEmptyAttribute NotEmptyAttribute = new NotEmptyAttribute();


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
			this.NotNullAttribute.OnParameterPrecondition(friendlyName, parameter, value);
			this.NotEmptyAttribute.OnParameterPrecondition(friendlyName, parameter, value);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(string friendlyName, object value)
		{
			this.NotNullAttribute.OnReturnValuePrecondition(friendlyName, value);
			this.NotEmptyAttribute.OnReturnValuePrecondition(friendlyName, value);
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether the value is an empty enumeration.</summary>
		/// <param name="value">The value to check.</param>
		protected bool IsEmpty(object value)
		{
			return value is IEnumerable && !(value as IEnumerable).Cast<object>().Any();
		}
	}
}
