using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract.Tests.Framework
{
	/// <summary>A test contract precondition which always fails and formats the contract metadata into its exception messages.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class AlwaysFailsAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Public methods
		*********/
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="Exception">This exception is always thrown.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			throw new Exception(String.Format("parameter={0}, value={1}, format={2}", parameter.ParameterName, value, parameter.MessageFormat));
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		/// <exception cref="Exception">This exception is always thrown.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			throw new Exception(String.Format("value={0}, format={1}", value, returnValue.MessageFormat));
		}
	}
}
