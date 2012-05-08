using System;
using System.Linq;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value must implement an expected type.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	public class HasTypeAttribute : Attribute, IParameterPrecondition, IReturnValuePrecondition
	{
		/*********
		** Public methods
		*********/
		/// <summary>The expected types that values must implement.</summary>
		public Type[] Types { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct a contract precondition that a value must implement an expected type.</summary>
		public HasTypeAttribute(params Type[] types)
		{
			this.Types = types ?? new Type[0];
		}

		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ArgumentException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(ParameterMetadata parameter, object value)
		{
			if (!this.HasType(value, this.Types))
				throw new ArgumentException(parameter.GetMessage(this.GetError(this.Types, value.GetType())), parameter.ParameterName);
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="returnValue">The return value metadata.</param>
		/// <param name="value">The return value.</param>
		/// <exception cref="InvalidOperationException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(ReturnValueMetadata returnValue, object value)
		{
			if (!this.HasType(value, this.Types))
				throw new InvalidOperationException(returnValue.GetMessage(this.GetError(this.Types, value.GetType())));
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Get whether the value implements an expected type.</summary>
		/// <param name="value">The value to check.</param>
		/// <param name="types">The expected types that the value must implement.</param>
		protected bool HasType(object value, Type[] types)
		{
			if (value == null)
				return true; // type is irrelevant in this case
			Type actualType = value.GetType();
			return types.Any(type => type.IsAssignableFrom(actualType));
		}

		/// <summary>Get a contract violation error message.</summary>
		/// <param name="types">The expected types that the value must implement.</param>
		/// <param name="actualType">The type of the value.</param>
		protected string GetError(Type[] types, Type actualType)
		{
			return String.Format(
				"must implement one of [{0}] (actually implements {1})",
				String.Join(", ", this.Types.Select(p => p.FullName)),
				actualType
			);
		}
	}
}
