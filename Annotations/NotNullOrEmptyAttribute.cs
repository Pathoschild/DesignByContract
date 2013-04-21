using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
    /// <summary>A contract precondition that a value is not null, or an empty string or collection.</summary>
    [AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
    [Serializable]
    [AppliesTo(typeof(string), typeof(IEnumerable<>))]
    public class NotNullOrEmptyAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
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
            if (this.IsNullOrEmpty(value))
                throw new ParameterContractException(parameter, "cannot be blank or consist entirely of whitespace");
        }

        /// <summary>Validate the requirement on a method or property return value.</summary>
        /// <param name="returnValue">The return value metadata.</param>
        /// <param name="value">The return value.</param>
        /// <exception cref="ReturnValueContractException">The contract requirement was not met.</exception>
        public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
        {
            if (this.IsNullOrEmpty(value))
                throw new ReturnValueContractException(returnValue, "cannot be blank or consist entirely of whitespace");
        }

        /*********
        ** Protected methods
        *********/
        /// <summary>Get whether the value is null, or an empty string or collection.</summary>
        /// <param name="value">The value to check.</param>
        protected bool IsNullOrEmpty(object value)
        {
            if (value == null)
                return true;

            if (value is string)
                return string.IsNullOrEmpty((string) value);
            if (value is IEnumerable)
                return IsEmpty((IEnumerable) value);

            return false;
        }

        static bool IsEmpty(IEnumerable e)
        {
            return !e.Cast<object>().Any();
        }
    }
}
