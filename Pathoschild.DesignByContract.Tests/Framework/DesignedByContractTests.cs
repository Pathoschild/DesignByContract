using System;
using NUnit.Framework;
using Pathoschild.DesignByContract.Tests.Framework.Annotated;

namespace Pathoschild.DesignByContract.Tests.Framework
{
	/// <summary>Unit tests which ensure <see cref="DesignedByContractAttribute"/> supports all expected scenarios.</summary>
	[TestFixture]
	public class DesignedByContractTests
	{
		/*********
		** Test cases
		*********/
		/// <summary>An NUnit test case in <see cref="DesignedByContractTests"/> for a parameter annotation.</summary>
		public class TestParameterAnnotationCaseAttribute : TestCaseAttribute
		{
			/// <summary>Construct an NUnit test case in <see cref="DesignedByContractTests"/> for a parameter annotation.</summary>
			/// <param name="value">The unit test parameter value and expected return.</param>
			/// <param name="typeName">The name of the type.</param>
			/// <param name="methodName">The name of the method.</param>
			/// <param name="parameterName">The name of the annotated parameter.</param>
			public TestParameterAnnotationCaseAttribute(bool value, string typeName, string methodName, string parameterName = "value")
				: base(value)
			{
				this.Result = value;
				this.ExpectedException = typeof(Exception);
				this.ExpectedMessage = String.Format("type={0}, method={1}, parameter={2}, value={3}", typeName, methodName, parameterName, value);
			}
		}

		/// <summary>An NUnit test case in <see cref="DesignedByContractTests"/> for a return value annotation.</summary>
		public class TestReturnValueAnnotationCaseAttribute : TestCaseAttribute
		{
			/// <summary>Construct an NUnit test case in <see cref="DesignedByContractTests"/> for a return value annotation.</summary>
			/// <param name="value">The unit test parameter value and expected return.</param>
			/// <param name="typeName">The name of the type.</param>
			/// <param name="methodName">The name of the method.</param>
			public TestReturnValueAnnotationCaseAttribute(bool value, string typeName, string methodName)
				: base(value)
			{
				this.Result = value;
				this.ExpectedException = typeof(Exception);
				this.ExpectedMessage = String.Format("type={0}, method={1}, value={2}", typeName, methodName, value);
			}
		}


		/*********
		** Unit tests
		*********/
		/***
		** Constructors
		***/
		[TestParameterAnnotationCase(true, "Sword", ".ctor")]
		[TestParameterAnnotationCase(false, "Sword", ".ctor")]
		public bool OnClass_ConstructorParameter(bool value)
		{
			new Sword(value);
			return value;
		}

		/***
		** Methods (instance)
		***/
		[TestParameterAnnotationCase(true, "Sword", "OnMethodParameter")]
		[TestParameterAnnotationCase(false, "Sword", "OnMethodParameter")]
		public bool OnClass_MethodParameter(bool value)
		{
			return new Sword().OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword", "OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "Sword", "OnMethodReturnValue")]
		public bool OnClass_MethodReturnValue(bool value)
		{
			return new Sword().OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword", "OnMethod")]
		[TestReturnValueAnnotationCase(false, "Sword", "OnMethod")]
		public bool OnClass_Method(bool value)
		{
			return new Sword().OnMethod(value);
		}

		/***
		** Methods (interface)
		***/
		[TestParameterAnnotationCase(true, "ISword", "OnMethodParameter")]
		[TestParameterAnnotationCase(false, "ISword", "OnMethodParameter")]
		public bool OnInterface_MethodParameter(bool value)
		{
			ISword sword = new InterfaceSword();
			return sword.OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "ISword", "OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "ISword", "OnMethodReturnValue")]
		public bool OnInterface_MethodReturnValue(bool value)
		{
			ISword sword = new InterfaceSword();
			return sword.OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "ISword", "OnMethod")]
		[TestReturnValueAnnotationCase(false, "ISword", "OnMethod")]
		public bool OnInterface_Method(bool value)
		{
			ISword sword = new InterfaceSword();
			return sword.OnMethod(value);
		}

		/***
		** Methods (static)
		***/
		[TestParameterAnnotationCase(true, "StaticSword", "OnMethodParameter")]
		[TestParameterAnnotationCase(false, "StaticSword", "OnMethodParameter")]
		public bool OnStaticClass_MethodParameter(bool value)
		{
			return StaticSword.OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword", "OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "StaticSword", "OnMethodReturnValue")]
		public bool OnStaticClass_MethodReturnValue(bool value)
		{
			return StaticSword.OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword", "OnMethod")]
		[TestReturnValueAnnotationCase(false, "StaticSword", "OnMethod")]
		public bool OnStaticClass_Method(bool value)
		{
			return StaticSword.OnMethod(value);
		}

		/***
		** Properties (classes)
		***/
		[TestReturnValueAnnotationCase(true, "Sword", "OnProperty")]
		[TestReturnValueAnnotationCase(false, "Sword", "OnProperty")]
		public bool OnClass_Property_Get(bool value)
		{
			return new Sword { _onProperty = value }.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword", "PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "Sword", "PrivateProperty")]
		public bool OnClass_Property_GetPrivate(bool value)
		{
			return new Sword { _privateProperty = value }.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword", "OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "Sword", "OnReadonlyProperty")]
		public bool OnClass_Property_GetReadonly(bool value)
		{
			return new Sword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword", "Item")]
		[TestReturnValueAnnotationCase(false, "Sword", "Item")]
		public bool OnClass_Property_GetIndexer(bool value)
		{
			return new Sword { _onIndexer = value }[value];
		}

		[TestParameterAnnotationCase(true, "Sword", "OnProperty")]
		[TestParameterAnnotationCase(false, "Sword", "OnProperty")]
		public bool OnClass_Property_Set(bool value)
		{
			return new Sword().OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword", "PrivateProperty")]
		[TestParameterAnnotationCase(false, "Sword", "PrivateProperty")]
		public bool OnClass_Property_SetPrivate(bool value)
		{
			return new Sword().OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword", "OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "Sword", "OnWriteonlyProperty")]
		public bool OnClass_Property_SetWriteonly(bool value)
		{
			return new Sword().OnWriteonlyProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword", "Item")]
		[TestParameterAnnotationCase(false, "Sword", "Item")]
		public bool OnClass_Property_SetIndexer(bool value)
		{
			return new Sword()[value] = value;
		}

		/***
		** Properties (interfaces)
		***/
		[TestReturnValueAnnotationCase(true, "ISword", "OnProperty")]
		[TestReturnValueAnnotationCase(false, "ISword", "OnProperty")]
		public bool OnInterface_Property_Get(bool value)
		{
			return new InterfaceSword { _onProperty = value }.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "ISword", "OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "ISword", "OnReadonlyProperty")]
		public bool OnInterface_Property_GetReadonly(bool value)
		{
			return new InterfaceSword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestReturnValueAnnotationCase(true, "ISword", "Item")]
		[TestReturnValueAnnotationCase(false, "ISword", "Item")]
		public bool OnInterface_Property_GetIndexer(bool value)
		{
			return new InterfaceSword { _onIndexer = value }[value];
		}

        [TestParameterAnnotationCase(true, "ISword", "OnProperty")]
        [TestParameterAnnotationCase(false, "ISword", "OnProperty")]
		public bool OnInterface_Property_Set(bool value)
		{
			return new InterfaceSword().OnProperty = value;
		}

        [TestParameterAnnotationCase(true, "ISword", "OnWriteonlyProperty")]
        [TestParameterAnnotationCase(false, "ISword", "OnWriteonlyProperty")]
		public bool OnInterface_Property_SetWriteonly(bool value)
		{
			return new InterfaceSword().OnWriteonlyProperty = value;
		}

        [TestParameterAnnotationCase(true, "ISword", "Item")]
        [TestParameterAnnotationCase(false, "ISword", "Item")]
		public bool OnInterface_Property_SetIndexer(bool value)
		{
			return new InterfaceSword()[value] = value;
		}

		/***
		** Properties (static)
		***/
		[TestReturnValueAnnotationCase(true, "StaticSword", "OnProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword", "OnProperty")]
		public bool OnStaticClass_Property_Get(bool value)
		{
			StaticSword._onProperty = value;
			return StaticSword.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "StaticSword", "PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword", "PrivateProperty")]
		public bool OnStaticClass_Property_GetPrivate(bool value)
		{
			StaticSword._privateProperty = value;
			return StaticSword.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "StaticSword", "OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword", "OnReadonlyProperty")]
		public bool OnStaticClass_Property_GetReadonly(bool value)
		{
			StaticSword._onReadonlyProperty = value;
			return StaticSword.OnReadonlyProperty;
		}

		[TestParameterAnnotationCase(true, "StaticSword", "OnProperty")]
		[TestParameterAnnotationCase(false, "StaticSword", "OnProperty")]
		public bool OnStaticClass_Property_Set(bool value)
		{
			return StaticSword.OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "StaticSword", "PrivateProperty")]
		[TestParameterAnnotationCase(false, "StaticSword", "PrivateProperty")]
		public bool OnStaticClass_Property_SetPrivate(bool value)
		{
			return StaticSword.OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "StaticSword", "OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "StaticSword", "OnWriteonlyProperty")]
		public bool OnStaticClass_Property_SetWriteonly(bool value)
		{
			return StaticSword.OnWriteonlyProperty = value;
		}
	}
}