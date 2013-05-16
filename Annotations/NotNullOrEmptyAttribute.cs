using System;
using System.Collections;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Constraints;

namespace Pathoschild.DesignByContract
{
	/// <summary>A contract precondition that a value is not null, or an empty string or collection.</summary>
	[Serializable]
	[AttributeUsage((AttributeTargets)(ConditionTargets.Parameter | ConditionTargets.ReturnValue))]
	[RequiresType(typeof(string), typeof(IEnumerable))]
	public class NotNullOrEmptyAttribute : NotEmptyAttribute
	{
		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		public NotNullOrEmptyAttribute()
		{
			this.RaiseErrorOnNull = false;
		}
	}
}