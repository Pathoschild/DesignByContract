using System;
using Pathoschild.DesignByContract.Framework;

namespace Pathoschild.DesignByContract
{
	/// <summary>Indicates that the value may be <c>null</c>.</summary>
	/// <remarks>This annotation is required for ReSharper to recognize <see cref="NotNullAttribute"/>.</remarks>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	public sealed class CanBeNullAttribute : Attribute { }
}
