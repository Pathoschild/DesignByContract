using System;

namespace Pathoschild.DesignByContract.Demo
{
	/// <summary>A simple class with contract annotations used in the console demo.</summary>
	[DesignedByContract]
	public class Sword
	{
		/// <summary>Inflict damage on the target.</summary>
		/// <param name="target">The target of the sword-swinging.</param>
		/// <returns>Returns a human-readable description of the action.</returns>
		/// <exception cref="ParameterContractException">The <paramref name="target"/> is <c>null</c> or blank.</exception>
		public string Hit([NotNull, NotBlank] string target)
		{
			return String.Format("You hit {0} with a sword!", target);
		}
	}
}