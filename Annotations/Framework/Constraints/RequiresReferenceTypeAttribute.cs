﻿using System;

namespace Pathoschild.DesignByContract.Framework.Constraints
{
	/// <summary>Constrains the usage of an annotation to reference types.</summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RequiresReferenceTypeAttribute : Attribute, IValidateContractUsageAttribute
	{
		/*********
		** Public methods
		*********/
		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="parameter">Represents a parameter annotated by a single contract annotation.</param>
		public string GetError(ParameterMetadata parameter)
		{
			if (!this.IsAllowedType(parameter.ParameterFullTypeName))
			{
				return String.Format(
					"This annotation is used incorrectly and will be ignored: {0} on the {1}::{2}({3}) parameter. The contract is only compatible with reference types.",
					parameter.Annotation.GetType(),
					parameter.TypeName,
					parameter.MethodName,
					parameter.Name
				);
			}
			return null;
		}

		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="returnValue">Represents a return value annotated by a single contract annotation.</param>
		public string GetError(ReturnValueMetadata returnValue)
		{
			if (!this.IsAllowedType(returnValue.ReturnFullTypeName))
			{
				return String.Format(
					"This annotation is used incorrectly and will be ignored: {0} on the {1}::{2} return value. The contract is only compatible with reference types.",
					returnValue.Annotation.GetType().Name,
					returnValue.TypeName,
					returnValue.MethodName
				);
			}
			return null;
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether a type is a reference type.</summary>
		/// <param name="typeName">The full name of the actual value type.</param>
		protected bool IsAllowedType(string typeName)
		{
			Type type = Type.GetType(typeName);
			return !type.IsValueType;
		}
	}
}