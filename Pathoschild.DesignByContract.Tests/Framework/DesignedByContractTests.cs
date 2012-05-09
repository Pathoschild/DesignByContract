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
			/// <param name="friendlyName">The friendly name of the invoked method or property.</param>
			/// <param name="parameterName">The name of the annotated parameter.</param>
			public TestParameterAnnotationCaseAttribute(bool value, string friendlyName, string parameterName = "value")
				: base(value)
			{
				this.Result = value;
				this.ExpectedException = typeof(Exception);
				this.ExpectedMessage = String.Format("parameter={0}, value={1}, format=Contract violation on parameter '{0}' of method '{2}': {{0}}", parameterName, value, friendlyName);
			}
		}

		/// <summary>An NUnit test case in <see cref="DesignedByContractTests"/> for a return value annotation.</summary>
		public class TestReturnValueAnnotationCaseAttribute : TestCaseAttribute
		{
			/// <summary>Construct an NUnit test case in <see cref="DesignedByContractTests"/> for a return value annotation.</summary>
			/// <param name="value">The unit test parameter value and expected return.</param>
			/// <param name="friendlyName">The friendly name of the invoked method or property.</param>
			public TestReturnValueAnnotationCaseAttribute(bool value, string friendlyName)
				: base(value)
			{
				this.Result = value;
				this.ExpectedException = typeof(Exception);
				this.ExpectedMessage = String.Format("value={0}, format=Contract violation on return value of method '{1}': {{0}}", value, friendlyName);
			}
		}


		/*********
		** Unit tests
		*********/
		/***
		** Constructors
		***/
		[TestParameterAnnotationCase(true, "Sword::.ctor")]
		[TestParameterAnnotationCase(false, "Sword::.ctor")]
		public bool OnClass_ConstructorParameter(bool value)
		{
			new Sword(value);
			return value;
		}

		/***
		** Methods (instance)
		***/
		[TestParameterAnnotationCase(true, "Sword::OnMethodParameter")]
		[TestParameterAnnotationCase(false, "Sword::OnMethodParameter")]
		public bool OnClass_MethodParameter(bool value)
		{
			return new Sword().OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethodReturnValue")]
		public bool OnClass_MethodReturnValue(bool value)
		{
			return new Sword().OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethod")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethod")]
		public bool OnClass_Method(bool value)
		{
			return new Sword().OnMethod(value);
		}

		/***
		** Methods (interface)
		***/
		[TestParameterAnnotationCase(true, "Sword::OnMethodParameter")]
		[TestParameterAnnotationCase(false, "Sword::OnMethodParameter")]
		public bool OnInterface_MethodParameter(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");

			ISword sword = new InterfaceSword();
			return sword.OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethodReturnValue")]
		public bool OnInterface_MethodReturnValue(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");

			ISword sword = new InterfaceSword();
			return sword.OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethod")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethod")]
		public bool OnInterface_Method(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");

			ISword sword = new InterfaceSword();
			return sword.OnMethod(value);
		}

		/***
		** Methods (static)
		***/
		[TestParameterAnnotationCase(true, "StaticSword::OnMethodParameter")]
		[TestParameterAnnotationCase(false, "StaticSword::OnMethodParameter")]
		public bool OnStaticClass_MethodParameter(bool value)
		{
			return StaticSword.OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "StaticSword::OnMethodReturnValue")]
		public bool OnStaticClass_MethodReturnValue(bool value)
		{
			return StaticSword.OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::OnMethod")]
		[TestReturnValueAnnotationCase(false, "StaticSword::OnMethod")]
		public bool OnStaticClass_Method(bool value)
		{
			return StaticSword.OnMethod(value);
		}

		/***
		** Properties (classes)
		***/
		[TestReturnValueAnnotationCase(true, "Sword::get_OnProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_OnProperty")]
		public bool OnClass_Property_Get(bool value)
		{
			return new Sword { _onProperty = value }.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword::get_PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_PrivateProperty")]
		public bool OnClass_Property_GetPrivate(bool value)
		{
			return new Sword { _privateProperty = value }.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword::get_OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_OnReadonlyProperty")]
		public bool OnClass_Property_GetReadonly(bool value)
		{
			return new Sword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword::get_Item")]
		[TestReturnValueAnnotationCase(false, "Sword::get_Item")]
		public bool OnClass_Property_GetIndexer(bool value)
		{
			return new Sword { _onIndexer = value }[value];
		}

		[TestParameterAnnotationCase(true, "Sword::set_OnProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_OnProperty")]
		public bool OnClass_Property_Set(bool value)
		{
			return new Sword().OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword::set_PrivateProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_PrivateProperty")]
		public bool OnClass_Property_SetPrivate(bool value)
		{
			return new Sword().OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword::set_OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_OnWriteonlyProperty")]
		public bool OnClass_Property_SetWriteonly(bool value)
		{
			return new Sword().OnWriteonlyProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword::set_Item")]
		[TestParameterAnnotationCase(false, "Sword::set_Item")]
		public bool OnClass_Property_SetIndexer(bool value)
		{
			return new Sword()[value] = value;
		}

		/***
		** Properties (interfaces)
		***/
		[TestReturnValueAnnotationCase(true, "InterfaceSword::get_OnProperty")]
		[TestReturnValueAnnotationCase(false, "InterfaceSword::get_OnProperty")]
		public bool OnInterface_Property_Get(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword { _onProperty = value }.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "InterfaceSword::get_PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "InterfaceSword::get_PrivateProperty")]
		public bool OnInterface_Property_GetPrivate(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword { _privateProperty = value }.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "InterfaceSword::get_OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "InterfaceSword::get_OnReadonlyProperty")]
		public bool OnInterface_Property_GetReadonly(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestReturnValueAnnotationCase(true, "InterfaceSword::get_Item")]
		[TestReturnValueAnnotationCase(false, "InterfaceSword::get_Item")]
		public bool OnInterface_Property_GetIndexer(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword { _onIndexer = value }[value];
		}

		[TestParameterAnnotationCase(true, "InterfaceSword::set_OnProperty")]
		[TestParameterAnnotationCase(false, "InterfaceSword::set_OnProperty")]
		public bool OnInterface_Property_Set(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword().OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "InterfaceSword::set_PrivateProperty")]
		[TestParameterAnnotationCase(false, "InterfaceSword::set_PrivateProperty")]
		public bool OnInterface_Property_SetPrivate(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword().OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "InterfaceSword::set_OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "InterfaceSword::set_OnWriteonlyProperty")]
		public bool OnInterface_Property_SetWriteonly(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword().OnWriteonlyProperty = value;
		}

		[TestReturnValueAnnotationCase(true, "InterfaceSword::set_Item")]
		[TestReturnValueAnnotationCase(false, "InterfaceSword::set_Item")]
		public bool OnInterface_Property_SetIndexer(bool value)
		{
			Assert.Inconclusive("Interface support is not yet implemented.");
			return new InterfaceSword()[value] = value;
		}

		/***
		** Properties (static)
		***/
		[TestReturnValueAnnotationCase(true, "StaticSword::get_OnProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword::get_OnProperty")]
		public bool OnStaticClass_Property_Get(bool value)
		{
			StaticSword._onProperty = value;
			return StaticSword.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::get_PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword::get_PrivateProperty")]
		public bool OnStaticClass_Property_GetPrivate(bool value)
		{
			StaticSword._privateProperty = value;
			return StaticSword.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::get_OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword::get_OnReadonlyProperty")]
		public bool OnStaticClass_Property_GetReadonly(bool value)
		{
			StaticSword._onReadonlyProperty = value;
			return StaticSword.OnReadonlyProperty;
		}

		[TestParameterAnnotationCase(true, "StaticSword::set_OnProperty")]
		[TestParameterAnnotationCase(false, "StaticSword::set_OnProperty")]
		public bool OnStaticClass_Property_Set(bool value)
		{
			return StaticSword.OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "StaticSword::set_PrivateProperty")]
		[TestParameterAnnotationCase(false, "StaticSword::set_PrivateProperty")]
		public bool OnStaticClass_Property_SetPrivate(bool value)
		{
			return StaticSword.OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "StaticSword::set_OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "StaticSword::set_OnWriteonlyProperty")]
		public bool OnStaticClass_Property_SetWriteonly(bool value)
		{
			return StaticSword.OnWriteonlyProperty = value;
		}
	}
}