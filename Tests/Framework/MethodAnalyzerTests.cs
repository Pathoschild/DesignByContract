using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Analysis;
using Pathoschild.DesignByContract.Framework.Constraints;

namespace Pathoschild.DesignByContract.Tests.Framework
{
	/// <summary>Unit tests which ensure <see cref="MethodAnalyzer"/> supports all expected scenarios.</summary>
	[TestFixture]
	public class MethodAnalyzerTests
	{
		/*********
		** Unit tests
		*********/
		[Test(Description = "Assert that the method analyzer detects the correct number of applicable contracts, given their constraints. That is, a contract that is not valid on the type should be included in the resulting metadata.")]
		[TestCase("Sample_NoAttributes", 0)]
		[TestCase("Sample_OneInvalid", 0)]
		[TestCase("Sample_OneValid", 1)]
		[TestCase("Sample_SeveralValid", 2)]
		[TestCase("Sample_OneValid_OneInvalid", 1)]
		[TestCase("Sample_OneValid_UntypedEnumerable", 1)]
		[TestCase("Sample_OneInvalid_UntypedEnumerable", 0)]
		[TestCase("Sample_OneValid_TypedEnumerable", 1)]
		[TestCase("Sample_OneInvalid_TypedEnumerable", 0)]
		[TestCase("Sample_OneInvalid_TypedEnumerableOnString", 0)]
		[TestCase("Sample_OneValid_ReferenceContractOnReferenceType", 1)]
		[TestCase("Sample_OneInvalid_ReferenceContractOnValueType", 0)]
		public void Analyzer_ReturnsValidNumberOfAnnotations_GivenConstraints(string methodName, int expectedContracts)
		{
			// analyze method
			MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			MethodAnalysis analysis = new MethodAnalyzer().AnalyzeMethod(method, false, emitWarnings: false);

			// assert
			Assert.AreEqual(expectedContracts, analysis.ParameterPreconditions.Length + analysis.ReturnValuePreconditions.Length);
			Assert.AreEqual(expectedContracts > 0, analysis.HasContract);
		}


		/*********
		** Sample annotated methods
		*********/
		private void Sample_NoAttributes(string s) { }
		private void Sample_OneValid([StringContract] string s) { }
		private void Sample_OneInvalid([StringContract] int e) { }
		private void Sample_SeveralValid([NoConstraint, StringContract] string s) { }
		private void Sample_OneValid_OneInvalid([StringContract, IntContract] string s) { }
		private void Sample_OneValid_UntypedEnumerable([UntypedEnumerableContract] IEnumerable<int> e) { }
		private void Sample_OneInvalid_UntypedEnumerable([UntypedEnumerableContract] string s) { }
		private void Sample_OneValid_TypedEnumerable([IntEnumerableContract] IEnumerable<int> e) { }
		private void Sample_OneInvalid_TypedEnumerable([IntEnumerableContract] IEnumerable<string> e) { }
		private void Sample_OneInvalid_TypedEnumerableOnString([IntEnumerableContract] string s) { }
		private void Sample_OneValid_ReferenceContractOnReferenceType([ReferenceContract] object o) { }
		private void Sample_OneInvalid_ReferenceContractOnValueType([ReferenceContract] int a) { }


		/*********
		** Sample contracts
		*********/
		/// <summary>A sample contract that can be used on any type.</summary>
		private class NoConstraintAttribute : Attribute, IParameterPrecondition
		{
			/// <summary>Validate the requirement on a single method parameter or property setter value.</summary>
			/// <param name="parameter">The parameter metadata.</param>
			/// <param name="value">The parameter value.</param>
			/// <exception cref="ParameterContractException">The contract requirement was not met.</exception>
			public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
		}


		/// <summary>A sample contract that can only be used on <see cref="string"/> types.</summary>
		[RequiresType(typeof(string))]
		private class StringContractAttribute : NoConstraintAttribute { }

		/// <summary>A sample contract that can only be used on <see cref="int"/> types.</summary>
		[RequiresType(typeof(int))]
		private class IntContractAttribute : NoConstraintAttribute { }

		/// <summary>A sample contract that can only be used on untyped <see cref="IEnumerable{T}"/> types.</summary>
		[RequiresType(typeof(IEnumerable<>))]
		private class UntypedEnumerableContractAttribute : NoConstraintAttribute { }

		/// <summary>A sample contract that can only be used on <see cref="IEnumerable{T}"/> (of <see cref="int"/>) types.</summary>
		[RequiresType(typeof(IEnumerable<int>))]
		private class IntEnumerableContractAttribute : NoConstraintAttribute { }

		/// <summary>A sample contract that can only be used on recence types.</summary>
		[RequiresReferenceType]
		private class ReferenceContractAttribute : NoConstraintAttribute { }
	}
}