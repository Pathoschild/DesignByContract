using Pathoschild.DesignByContract.Exceptions;

namespace Pathoschild.DesignByContract.Framework
{
	// <summary>Represents a contract annotation for a requirement on a method parameter or property setter value.</summary>
	public interface IParameterPrecondition
	{
		/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
		/// <param name="parameter">The parameter metadata.</param>
		/// <param name="value">The parameter value.</param>
		/// <exception cref="ParameterContractException">The contract requirement was not met.</exception>
		void OnParameterPrecondition(ParameterMetadata parameter, object value);
	}
}