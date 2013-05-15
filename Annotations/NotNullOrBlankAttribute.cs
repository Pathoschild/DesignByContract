using System;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Constraints;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value not be a string that is null or consists entirely of whitespace.</summary>
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[Serializable]
	[RequiresType(typeof(string))]
	public class NotNullOrBlankAttribute : NotBlankAttribute
	{
		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		public NotNullOrBlankAttribute()
		{
			this.RaiseErrorOnNull = true;
		}
	}
}