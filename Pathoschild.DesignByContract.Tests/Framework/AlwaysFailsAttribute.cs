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
			throw new Exception(String.Format("type={0}, method={1}, parameter={2}, value={3}", parameter.TypeName, parameter.MethodName, parameter.Name, value));
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="Exception">The contract requirement was not met.</exception>
		/// <exception cref="Exception">This exception is always thrown.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			throw new Exception(String.Format("type={0}, method={1}, value={2}", returnValue.TypeName, returnValue.MethodName, value));
		}
	}
}
