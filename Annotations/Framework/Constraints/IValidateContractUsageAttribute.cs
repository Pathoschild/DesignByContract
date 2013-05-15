namespace Pathoschild.DesignByContract.Framework.Constraints
{
	/// <summary>An attribute that constrains how an annotation can be used.</summary>
	public interface IValidateContractUsageAttribute
	{
		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="parameter">Represents a parameter annotated by a single contract annotation.</param>
		string GetError(ParameterMetadata parameter);

		/// <summary>Get an error message indicating why the usage is invalid (or <c>null</c> if usage is valid).</summary>
		/// <param name="returnValue">Represents a return value annotated by a single contract annotation.</param>
		string GetError(ReturnValueMetadata returnValue);
	}
}