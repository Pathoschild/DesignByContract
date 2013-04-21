using System;
using System.Linq;

namespace Pathoschild.DesignByContract.Framework
{
    /// <summary>
    /// Used internally to denote to what types (and their subclasses) a contract can apply
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AppliesToAttribute : Attribute
    {
        /// <summary>
        /// The list of <see cref="Type"/>s
        /// </summary>
        public Type[] Types { get; set; }

        /// <summary>
        /// True if the contract cannot be applied to value types
        /// </summary>
        public bool RejectValueTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstType">At least one <see cref="Type"/> must be specified.</param>
        /// <param name="extraTypes">Extra types that it applies to</param>
        public AppliesToAttribute(Type firstType, params Type[] extraTypes)
        {
            Types = new[] { firstType }.Concat(extraTypes).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        protected AppliesToAttribute()
        {
            
        }
    }

    /// <summary>
    /// Used internally to denote that a contract can apply to reference types only
    /// </summary>
    public class AppliesToReferenceTypes : AppliesToAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeConstraints">Any additional type constraints</param>
        public AppliesToReferenceTypes(params Type[] typeConstraints)
        {
            Types = typeConstraints;
            RejectValueTypes = true;
        }
    }
}
