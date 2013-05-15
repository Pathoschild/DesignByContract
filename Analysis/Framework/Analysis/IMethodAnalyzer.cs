using System.Reflection;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
	/// <summary>Reflects methods and properties for contract analysis.</summary>
	public interface IMethodAnalyzer
	{
		/// <summary>Analyze the contract annotations on a methods.</summary>
		/// <param name="method">The method to analyze.</param>
		/// <param name="inheritContract">Whether to inherit attributes from base types or interfaces.</param>
		/// <param name="emitWarnings">Whether to emit build warnings during analysis that indicate to the user where annotations are incorrectly used and will be ignored. This can only be used during compile-time analysis.</param>
		MethodAnalysis AnalyzeMethod(MethodBase method, bool inheritContract, bool emitWarnings = true);
	}
}