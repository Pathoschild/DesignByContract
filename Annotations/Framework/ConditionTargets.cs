using System;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Specifies the application elements on which it is valid to apply a contract annotation based on the condition types it implements.</summary>
	[Flags]
	public enum ConditionTargets
	{
		/// <summary>Annotation that implements <see cref="IParameterPrecondition"/>.</summary>
		Parameter = AttributeTargets.GenericParameter | AttributeTargets.Parameter,

		/// <summary>Annotation that implements <see cref="IReturnValuePrecondition"/>.</summary>
		ReturnValue = AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.ReturnValue,

		/// <summary>Annotation that is valid on every supported application element.</summary>
		All = AttributeTargets.GenericParameter | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue
	}
}