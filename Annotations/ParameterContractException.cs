using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>The exception that is thrown when a value provided to a method parameter or property setter violations its contract.</summary>
	public class ParameterContractException : ArgumentException
	{
		/// <summary>Construct an instance.</summary>
		/// <param name="typeName">The name of the type.</param>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameterName">The name of the validated parameter.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public ParameterContractException(string typeName, string methodName, string parameterName, string error)
			: base(
				String.Format("Contract violation on parameter '{0}' of method '{1}::{2}': {3}", parameterName, typeName, methodName, error),
				parameterName
			) { }

		/// <summary>Construct an instance.</summary>
		/// <param name="parameter">The validated parameter.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public ParameterContractException(ParameterMetadata parameter, string error)
			: this(parameter.TypeName, parameter.MethodName, parameter.Name, error) { }
	}
}