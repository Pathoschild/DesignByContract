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
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="parameter">Metadata about the input parameter to check.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="ArgumentException">The contract requirement was not met.</exception>
		public void OnParameterPrecondition(string friendlyName, ParameterMetadata parameter, object value)
		{
			if (!this.HasType(value, this.Types))
			{
				throw new ArgumentException(
					String.Format(
						"The value must implement one of [{0}] for parameter '{1}' of method {2}. (It actually implements {3}.)",
						String.Join(", ", this.Types.Select(p => p.FullName)),
						parameter.Parameter.Name,
						friendlyName,
						value.GetType()
					),
					parameter.Parameter.Name
				);
			}
		}

		/// <summary>Validate the requirement on a method or property return value.</summary>
		/// <param name="friendlyName">A human-readable name representing the method being validated for use in exception messages.</param>
		/// <param name="value">The value to check.</param>
		/// <exception cref="InvalidOperationException">The contract requirement was not met.</exception>
		public void OnReturnValuePrecondition(string friendlyName, object value)
		{
			if (!this.HasType(value, this.Types))
			{
				throw new InvalidOperationException(
					String.Format(
						"The return value must implement one of [{0}] for method {1}. (It actually implements {2}.)",
						String.Join(", ", this.Types.Select(p => p.FullName)),
						friendlyName,
						value.GetType()
					)
				);
			}
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
	}
}
