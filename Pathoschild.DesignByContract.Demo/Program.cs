using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pathoschild.DesignByContract.Demo
{
	[DesignedByContract]
	public class Sword
	{
		public string Hit([NotNull] string actor, [NotNull] string target)
		{
			return String.Format("{0} hit {1} with a sword!", actor, target);
		}

		[NotNull]
		public string GetValue(string value)
		{
			return value;
		}
	}

	class Program
	{
static void Main(string[] args)
		{
			//                "1234567890123456789012345678901234567890123456789012345678901234567890123456789"
			Console.WriteLine("We have a Sword class with a Hit method which, by contract, can never be passed");
			Console.WriteLine("null values.");
			Console.WriteLine();

			Console.WriteLine("Let's pass it some valid values.");
			PrintInvocation(sword => sword.Hit("Jake", "teh baddies"));
			Console.WriteLine();
			
			Console.WriteLine("The contract says you can't give it null arguments; what happens if you do?");
			PrintInvocation(sword => sword.Hit("Jake", null));
			Console.WriteLine();

			Console.WriteLine("Okay, so that threw an exception. What about a blank value?");
			PrintInvocation(sword => sword.Hit("Jake", ""));
			Console.WriteLine();

			Console.WriteLine("Any invalid values passed to Sword::Hit will immediately throw an exception;");
			Console.WriteLine("this means you won't get mysterious NullReferenceExceptions that you need to");
			Console.WriteLine("hunt around for.");
			Console.WriteLine();

			Console.WriteLine("You can do the same for return types, which makes your code more refactor-safe.");
			Console.WriteLine("If a new (or old) programmer changes the return behaviour without realizing that");
			Console.WriteLine("code depends on a non-null value, it will fail as soon as it returns.");
			Console.WriteLine();

			Console.WriteLine("Let's see. The Sword class also has a GetValue method, just for our demo");
			Console.WriteLine("purposes. It returns whatever you give it without validating it, but the return");
			Console.WriteLine("type itself is guaranteed to never be null. Here it is with a valid value:");
			PrintInvocation(sword => sword.GetValue("Jake"));
			Console.WriteLine();

			Console.WriteLine("And here it is with a null value:");
			PrintInvocation(sword => sword.GetValue(null));
			Console.WriteLine();

			Console.WriteLine("And that concludes this little demo session. Feel free to read the documentation");
			Console.WriteLine("or look at the unit tests for more information, or ask a question on the GitHub page.");
		}

		static void PrintInvocation(Expression<Func<Sword, string>> action)
		{
			Console.WriteLine("> {0}", action.Body);
			try
			{
				Sword sword = new Sword();
				string result = action.Compile().Invoke(sword);
				Console.WriteLine("	\"{0}\"", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine("	{0}: {1}", ex.GetType().Name, String.Join("\n	", ex.Message.Split('\n')));
			}
		}
	}
}
