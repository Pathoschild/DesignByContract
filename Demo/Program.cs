using System;
using System.Linq;

namespace Pathoschild.DesignByContract.Demo
{
	/// <summary>A simple demonstration of design by contract validation using the annotated <see cref="Sword"/>.</summary>
	public class Program
	{
		/// <summary>Execute the demo.</summary>
		static void Main()
		{
			Sword sword = new Sword();
			
			//                "1234567890123456789012345678901234567890123456789012345678901234567890123456789"
			Console.WriteLine("Hi!");
			Console.WriteLine();
			Console.WriteLine("We have a Sword class with a Hit method, whose argument is annotated with the");
			Console.WriteLine("[NotBlank] precondition. The class itself has no validation logic at all.");
			
			Console.WriteLine();
			Console.WriteLine("Try entering some values to pass to this argument. Type \"q\" to continue.");
			PrintDemo(sword.Hit);

			Console.WriteLine();
			Console.WriteLine("The argument validation was handled entirely by the [NotBlank] annotation. You");
			Console.WriteLine("can annotate methods, properties, parameters, and return values. You can use any");
			Console.WriteLine("of the default annotations (like [NotNull], [NotBlank], or [NotEmpty]) or easily");
			Console.WriteLine("create your own annotations just by implementing an interface on a custom");
			Console.WriteLine("attribute.");
			Console.WriteLine();

			Console.WriteLine("That concludes this little demonstration. Feel free to look through the unit tests,");
			Console.WriteLine("read the documentation, or ask a question on the GitHub project.");
			Console.WriteLine();
			Console.WriteLine("Bye!");
		}

		static void PrintDemo(Func<string, string> action)
		{
			for(string input = Console.ReadLine(); input.ToLower().Trim() != "q"; input = Console.ReadLine())
			{
				Console.WriteLine("> {0}", GetInvocationResult(() => action(input)));
				Console.WriteLine();
			} 
		}

		/// <summary>Print a textual representation of the method invocation to the console along with the return value or thrown exception.</summary>
		/// <param name="action">The method invocation to print.</param>
		static string GetInvocationResult(Func<string> action)
		{
			try
			{
				return String.Format("\"{0}\"", action());
			}
			catch (Exception ex)
			{
				return String.Format("{0}: {1}", ex.GetType().Name, ex.Message.Split('\n').First());
			}
		}
	}
}
