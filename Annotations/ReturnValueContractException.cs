using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>The exception that is thrown when a value provided to a method parameter or property setter violations its contract.</summary>
	public class ReturnValueContractException : InvalidOperationException
	{
		/// <summary>Construct an instance.</summary>
		/// <param name="typeName">The name of the type.</param>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public ReturnValueContractException(string typeName, string methodName, string error)
			: base(
				String.Format("Contract violation on return value of method '{0}::{1}': {2}", typeName, methodName, error)
			) { }

		/// <summary>Construct an instance.</summary>
		/// <param name="parameter">The validated parameter.</param>
		/// <param name="error">The brief description of the contract violation.</param>
		public ReturnValueContractException(ReturnValueMetadata parameter, string error)
			: this(parameter.TypeName, parameter.MethodName, error) { }
	}
}