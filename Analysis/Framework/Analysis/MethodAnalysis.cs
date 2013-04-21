using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
    /// <summary>Contains contract metadata about an annotated method or property.</summary>
    [Serializable, DataContract]
    public struct MethodAnalysis
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The contract requirements for each method parameter or property setter value.</summary>
        [DataMember]
        public ParameterMetadata[] ParameterPreconditions { get; set; }

        /// <summary>The contract requirements on a method or property return value.</summary>
        [DataMember]
        public ReturnValueMetadata[] ReturnValuePreconditions { get; set; }

        /// <summary>Whether the method has contract annotations to enforce.</summary>
        [IgnoreDataMember]
        public bool HasContract
        {
            get { return this.ParameterPreconditions.Any() || this.ReturnValuePreconditions.Any(); }
        }

        /// <summary>
        /// Determines whether the current contract can be applied to the target
        /// </summary>
        /// <returns>True if the contract can be applied or false otherwise</returns>
        public bool DetermineIfContractIsApplicable(out IEnumerable<string> errors)
        {
            var contracts = ParameterPreconditions.Select(AsContractApplication)
                .Concat(ReturnValuePreconditions.Select(AsContractApplication))
                .ToArray();

            List<string> errorList = new List<string>();
            bool allValid = contracts.All(contract =>
            {
                if (contract.RejectValueTypes && contract.TargetType.IsValueType)
                {
                    errorList.Add(
                        string.Format("{0} cannot be applied to {1}({2}) because it is a value type", 
                            contract.Type.Name, contract.MethodName, contract.Name));
                    return false;
                }

                bool hasNoTypeConstraints = !contract.ValidTypes.Any();
                bool allConstraintsValid = contract.ValidTypes.Any(appliedType =>
                {
                    if(!CanApply(contract.TargetType, appliedType))
                    {
                        errorList.Add(
                            string.Format("{0} cannot be applied to {1}({2}) because it does not satisfy the constract's type constraints",
                                contract.Type.Name, contract.MethodName, contract.Name));
                        return false;
                    }
                    return true;
                });
                
                return hasNoTypeConstraints || allConstraintsValid;
            });

            if(!allValid)
            {
                // errors are generated locally (without knowledge of other valid types)
                // so, only assign errors if all validations failed
                errors = errorList;
                return false;
            }
            errors = new string[0];
            return true;
        }

        private static bool CanApply(Type valueType, Type appliedType)
        {
            // exact match => ok
            if (valueType == appliedType)
                return true;

            // both are generics, maybe there is a match
            if (valueType.IsGenericType && appliedType.IsGenericType)
            {
                // if the applied type is a generic type def, allow any type param
                if (appliedType.IsGenericTypeDefinition)
                    return valueType.GetGenericTypeDefinition() == appliedType.GetGenericTypeDefinition();

                // otherwise, match exact type
                return valueType == appliedType;
            }

            return false;
        }

        private static ContractApplication AsContractApplication(ParameterMetadata p)
        {
            var applications = p.Annotation.GetType()
                .GetCustomAttributes(typeof(AppliesToAttribute), false)
                .Cast<AppliesToAttribute>()
                .ToArray();
            return new ContractApplication
            {
                MethodName = p.MethodName,
                Name = p.Name,
                Type = p.Annotation.GetType(),
                TargetType = Type.GetType(p.ParameterFullTypeName),
                ValidTypes = applications
                    .SelectMany(attr => attr.Types)
                    .ToArray(),
                RejectValueTypes = applications.Any(app => app.RejectValueTypes)
            };
        }

        private static ContractApplication AsContractApplication(ReturnValueMetadata p)
        {
            var applications = p.Annotation.GetType()
                .GetCustomAttributes(typeof(AppliesToAttribute), false)
                .Cast<AppliesToAttribute>()
                .ToArray();
            return new ContractApplication
            {
                MethodName = p.MethodName,
                Name = "Return Value",
                Type = p.Annotation.GetType(),
                TargetType = Type.GetType(p.ReturnFullTypeName),
                ValidTypes = applications
                    .SelectMany(attr => attr.Types)
                    .ToArray(),
                RejectValueTypes = applications.Any(app => app.RejectValueTypes)
            };
        }

        private class ContractApplication
        {
            public string MethodName { get; set; }
            public string Name { get; set; }
            public Type Type { get; set; }
            public Type TargetType { get; set; }
            public Type[] ValidTypes { get; set; }
            public bool RejectValueTypes { get; set; }
        }

        /*********
        ** Public methods
        *********/
        /// <summary>Get a human-readable representation of the method analysis.</summary>
        public override string ToString()
        {
            return String.Format(
                "Analysis: {{ HasContract={0}, ParameterPreconditions=[{1}], ReturnValuePreconditions=[{2}] }}",
                this.HasContract,
                String.Join(", ", this.ParameterPreconditions.Select(p => String.Format(@"{{TypeName='{0}', MethodName='{1}', Parameter='{2}', Annotation={3}}}", p.TypeName, p.MethodName, p.Name, p.Annotation.GetType().Name))),
                String.Join(", ", this.ReturnValuePreconditions.Select(p => String.Format(@"{{TypeName='{0}', MethodName='{1}', Annotation={2}}}", p.TypeName, p.MethodName, p.Annotation.GetType().Name)))
            );
        }
    }
}
