﻿using System;
using System.Linq;

namespace Pathoschild.DesignByContract.Framework.Constraints
{
	/// <summary>Constrains the usage of an annotation to specific types.</summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RequiresTypeAttribute : Attribute, IValidateContractUsageAttribute
	{
		/*********
		** Accessors
		*********/
		/// <summary>The valid types the contract can annotate.</summary>
		public Type[] AllowedTypes { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <summary>The valid types the contract can annotate.</summary>
		public RequiresTypeAttribute(params Type[] allowedTypes)
		{
			this.AllowedTypes = allowedTypes;
		}

		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="parameter">Represents a parameter annotated by a single contract annotation.</param>
		public string GetError(ParameterMetadata parameter)
		{
			if (!this.IsAllowedType(parameter.ParameterQualifiedType, parameter.ParameterTypeIsUnknown))
			{
				return String.Format(
					"This annotation is used incorrectly and will be ignored: {0} on the {1}::{2}({3}) parameter. The contract is only compatible with {4} types.",
					parameter.Annotation.GetType().Name,
					parameter.TypeName,
					parameter.MethodName,
					parameter.Name,
					String.Join(" or ", this.AllowedTypes.Select(p => p.Name))
				);
			}
			return null;
		}

		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="returnValue">Represents a return value annotated by a single contract annotation.</param>
		public string GetError(ReturnValueMetadata returnValue)
		{
			if (!this.IsAllowedType(returnValue.ReturnTypeQualifiedName, returnValue.ReturnTypeIsUnknown))
			{
				return String.Format(
					"This annotation is used incorrectly and will be ignored: {0} on the {1}::{2} return value. The contract is only compatible with {3} types.",
					returnValue.Annotation.GetType().Name,
					returnValue.TypeName,
					returnValue.MethodName,
					String.Join(" or ", this.AllowedTypes.Select(p => p.Name))
				);
			}
			return null;
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether a type is compatible with the <see cref="AllowedTypes"/>.</summary>
		/// <param name="typeName">The full name of the actual value type.</param>
		/// <param name="typeIsUnknown">Whether the type is not known at compile-time. This occurs when the type is generic and the type isn't specified in code.</param>
		protected bool IsAllowedType(string typeName, bool typeIsUnknown)
		{
			if (typeIsUnknown)
				return true; // don't validate the type at compile-time
			Type type = Type.GetType(typeName);
			return this.IsAllowedType(type);
		}

		/// <summary>Get whether a type is compatible with the <see cref="AllowedTypes"/>.</summary>
		/// <param name="type">The actual value type.</param>
		protected bool IsAllowedType(Type type)
		{
			foreach (Type allowedType in this.AllowedTypes)
			{
				// type is equivalent
				if (allowedType.IsAssignableFrom(type))
					return true;

				// generic type is equivalent
				if (allowedType.IsGenericType && allowedType.IsGenericTypeDefinition && type.IsGenericType)
					return allowedType.GetGenericTypeDefinition() == type.GetGenericTypeDefinition();
			}

			return false;
		}
	}
}