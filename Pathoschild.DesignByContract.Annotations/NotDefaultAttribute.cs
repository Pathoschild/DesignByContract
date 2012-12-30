using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
    /// <summary>A contract precondition that a value is not equal to <c>default(Type)</c>.</summary>
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
            if (value == null || value.Equals(GetDefaultValue(value.GetType())))
                throw new ParameterContractException(parameter, "cannot have default value");
        }

        /// <summary>Validate the requirement on a method or property return value.</summary>
        /// <param name="returnValue">The return value metadata.</param>
        /// <param name="value">The return value.</param>
        /// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
        public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
        {
            if (value == null || value.Equals(GetDefaultValue(value.GetType())))
                throw new ReturnValueContractException(returnValue, "cannot have default value");
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);
            return null;
        }
    }
}