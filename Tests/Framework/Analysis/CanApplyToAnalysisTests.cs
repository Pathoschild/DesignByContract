using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pathoschild.DesignByContract.Framework;
using Pathoschild.DesignByContract.Framework.Analysis;

namespace Pathoschild.DesignByContract.Tests.Framework.Analysis
{
    [TestFixture]
    public class CanApplyToAnalysisTests
    {
        [Test]
        public void NoContracts_ReturnsTrue()
        {
            AssertAnalyze(true, 0, (Action<string>)Method_NoAttributes);
        }

        [Test]
        public void SingleContract_OnCorrectType_ReturnsTrue()
        {
            AssertAnalyze(true, 1, (Action<string>)SingleContract_OnCorrectType);
        }

        [Test]
        public void SingleContract_OnIncorrectType_ReturnsFalse()
        {
            AssertAnalyze(false, 1, (Action<int>)SingleContract_OnIncorrectType);
        }

        [Test]
        public void MultipleContracts_AllCanApply_ReturnsTrue()
        {
            AssertAnalyze(true, 2, (Action<string>)MultipleContracts_AllCanApply);
        }

        [Test]
        public void MultipleContracts_SomeDoNotApply_ReturnsFalse()
        {
            AssertAnalyze(false, 2, (Action<string>)MultipleContracts_SomeDoNotApply);
        }

        [Test]
        public void UntypedGenericContract_OnCorrectType_ReturnsTrue()
        {
            AssertAnalyze(true, 1, (Action<IEnumerable<int>>)UntypedGenericContract_OnCorrectType);
        }

        [Test]
        public void UntypedGenericContract_OnIncorrectType_ReturnsFalse()
        {
            AssertAnalyze(false, 1, (Action<string>)UntypedGenericContract_OnIncorrectType);
        }

        [Test]
        public void TypedGenericContract_OnCorrectType_ReturnsTrue()
        {
            AssertAnalyze(true, 1, (Action<IEnumerable<int>>)TypedGenericContract_OnCorrectType);
        }

        [Test]
        public void TypedGenericContract_OnIncorrectType_ReturnsFalse()
        {
            AssertAnalyze(false, 1, (Action<string>)TypedGenericContract_OnIncorrectType);
        }

        [Test]
        public void TypedGenericContract_OnIncorrectTypeParam_ReturnsFalse()
        {
            AssertAnalyze(false, 1, (Action<IEnumerable<string>>)TypedGenericContract_OnIncorrectTypeParam);
        }

        [Test]
        public void ReferenceContract_OnReferenceType_ReturnsTrue()
        {
            AssertAnalyze(true, 1, (Action<object>)ReferenceContract_OnReferenceType);
        }

        [Test]
        public void ReferenceContract_OnValueType_ReturnsFalse()
        {
            AssertAnalyze(false, 1, (Action<int>)ReferenceContract_OnValueType);
        }

        private static void Method_NoAttributes(string s) { }
        private static void SingleContract_OnCorrectType([StringContract] string s) { }
        private static void SingleContract_OnIncorrectType([StringContract] int e) { }
        private static void MultipleContracts_AllCanApply([AnyTypeContract, StringContract] string s) { }
        private static void MultipleContracts_SomeDoNotApply([StringContract, IntContract] string s) { }
        private static void UntypedGenericContract_OnCorrectType([UntypedEnumerableContract] IEnumerable<int> e) { }
        private static void UntypedGenericContract_OnIncorrectType([UntypedEnumerableContract] string s) { }
        private static void TypedGenericContract_OnCorrectType([IntEnumerableContract] IEnumerable<int> e) { }
        private static void TypedGenericContract_OnIncorrectType([IntEnumerableContract] string s) { }
        private static void TypedGenericContract_OnIncorrectTypeParam([IntEnumerableContract] IEnumerable<string> e) { }
        private static void ReferenceContract_OnReferenceType([ReferenceContract] object o) { }
        private static void ReferenceContract_OnValueType([ReferenceContract] int a) { }

        #region TestAttributes
        class AnyTypeContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }

        [AppliesTo(typeof(string))]
        class StringContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }

        [AppliesTo(typeof(int))]
        class IntContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }

        [AppliesTo(typeof(IEnumerable<>))]
        class UntypedEnumerableContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }

        [AppliesTo(typeof(IEnumerable<int>))]
        class IntEnumerableContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }

        [AppliesToReferenceTypes]
        class ReferenceContractAttribute : Attribute, IParameterPrecondition
        {
            public void OnParameterPrecondition(ParameterMetadata parameter, object value) { }
        }
        #endregion

        #region Test Helpers
        private static void AssertAnalyze(bool expectedIsApplicable, int expectedContracts,
            Delegate method)
        {
            var analysis = new MethodAnalyzer().AnalyzeMethod(method.Method, false);
            IEnumerable<string> errors;
            Assert.AreEqual(expectedContracts, analysis.ParameterPreconditions.Length + analysis.ReturnValuePreconditions.Length);
            Assert.AreEqual(expectedIsApplicable, analysis.DetermineIfContractIsApplicable(out errors));
        }

        #endregion
    }
}
