using System;

namespace Pathoschild.DesignByContract.Demo
{
	/// <summary>A simple class with contract annotations used in the console demo.</summary>
	[DesignedByContract]
	public class Sword
	{
		/// <summary>Inflict damage on the target.</summary>
		/// <param name="actor">The person swinging the sword.</param>
		/// <param name="target">The target of the sword-swinging.</param>
		/// <returns>Returns a human-readable description of the action.</returns>
		public string Hit([NotNull] string actor, [NotNull] string target)
		{
			return String.Format("{0} hit {1} with a sword!", actor, target);
		}

		/// <summary>Echo the input value.</summary>
		/// <param name="value">The value to echo.</param>
		/// <returns>The input <paramref name="value"/>.</returns>
		[NotNull]
		public string GetValue(string value)
		{
			return value;
		}
	}
}