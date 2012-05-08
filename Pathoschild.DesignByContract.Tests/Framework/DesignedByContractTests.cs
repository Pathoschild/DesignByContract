using System;
using NUnit.Framework;

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
		public bool OnConstructorParameter(bool value)
		{
			new Sword(value);
			return value;
		}

		/***
		** Methods (instance)
		***/
		[TestParameterAnnotationCase(true, "Sword::OnMethodParameter")]
		[TestParameterAnnotationCase(false, "Sword::OnMethodParameter")]
		public bool OnMethodParameter(bool value)
		{
			return new Sword().OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethodReturnValue")]
		public bool OnMethodReturnValue(bool value)
		{
			return new Sword().OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "Sword::OnMethod")]
		[TestReturnValueAnnotationCase(false, "Sword::OnMethod")]
		public bool OnMethod(bool value)
		{
			return new Sword().OnMethod(value);
		}

		/***
		** Methods (static)
		***/
		[TestParameterAnnotationCase(true, "StaticSword::OnMethodParameter")]
		[TestParameterAnnotationCase(false, "StaticSword::OnMethodParameter")]
		public bool OnStaticMethodParameter(bool value)
		{
			return StaticSword.OnMethodParameter(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::OnMethodReturnValue")]
		[TestReturnValueAnnotationCase(false, "StaticSword::OnMethodReturnValue")]
		public bool OnStaticMethodReturnValue(bool value)
		{
			return StaticSword.OnMethodReturnValue(value);
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::OnMethod")]
		[TestReturnValueAnnotationCase(false, "StaticSword::OnMethod")]
		public bool OnStaticMethod(bool value)
		{
			return StaticSword.OnMethod(value);
		}

		/***
		** Property getters
		***/
		[TestReturnValueAnnotationCase(true, "Sword::get_OnProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_OnProperty")]
		public bool OnProperty_Get(bool value)
		{
			return new Sword { _onProperty = value }.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword::get_PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_PrivateProperty")]
		public bool OnProperty_GetPrivate(bool value)
		{
			return new Sword { _privateProperty = value }.OnPrivateProperty;
		}

		[TestReturnValueAnnotationCase(true, "Sword::get_OnReadonlyProperty")]
		[TestReturnValueAnnotationCase(false, "Sword::get_OnReadonlyProperty")]
		public bool OnProperty_GetReadonly(bool value)
		{
			return new Sword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestParameterAnnotationCase(true, "Sword::set_OnProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_OnProperty")]
		public bool OnProperty_Set(bool value)
		{
			return new Sword().OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword::set_PrivateProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_PrivateProperty")]
		public bool OnProperty_SetPrivate(bool value)
		{
			return new Sword().OnPrivateProperty = value;
		}

		[TestParameterAnnotationCase(true, "Sword::set_OnWriteonlyProperty")]
		[TestParameterAnnotationCase(false, "Sword::set_OnWriteonlyProperty")]
		public bool OnProperty_SetWriteonly(bool value)
		{
			return new Sword().OnWriteonlyProperty = value;
		}

		/***
		** Properties (static)
		***/
		[TestReturnValueAnnotationCase(true, "StaticSword::get_OnProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword::get_OnProperty")]
		public bool OnProperty_StaticGet(bool value)
		{
			StaticSword._onProperty = value;
			return StaticSword.OnProperty;
		}

		[TestReturnValueAnnotationCase(true, "StaticSword::get_PrivateProperty")]
		[TestReturnValueAnnotationCase(false, "StaticSword::get_PrivateProperty")]
		public bool OnProperty_StaticGetPrivate(bool value)
		{
			StaticSword._privateProperty = value;
			return StaticSword.OnPrivateProperty;
		}

		[TestParameterAnnotationCase(true, "StaticSword::set_OnProperty")]
		[TestParameterAnnotationCase(false, "StaticSword::set_OnProperty")]
		public bool OnProperty_StaticSet(bool value)
		{
			return StaticSword.OnProperty = value;
		}

		[TestParameterAnnotationCase(true, "StaticSword::set_PrivateProperty")]
		[TestParameterAnnotationCase(false, "StaticSword::set_PrivateProperty")]
		public bool OnProperty_StaticSetPrivate(bool value)
		{
			return StaticSword.OnPrivateProperty = value;
		}


		/*********
		** Objects
		*********/
		/// <summary>A class containing annotated code.</summary>
		[DesignedByContract]
		public class Sword
		{
			/*********
			** Properties
			*********/
			[AlwaysFails]
			private bool PrivateProperty
			{
				get { return this._privateProperty; }
				set { this._privateProperty = value; }
			}
			public bool _privateProperty;


			/*********
			** Accessors
			*********/
			[AlwaysFails]
			public bool OnProperty
			{
				get { return this._onProperty; }
				set { this._onProperty = value; }
			}
			public bool _onProperty;

			[AlwaysFails]
			public bool OnReadonlyProperty
			{
				get { return this._onReadonlyProperty; }
			}
			public bool _onReadonlyProperty;

			[AlwaysFails]
			public bool OnWriteonlyProperty
			{
				set { this._onWriteonlyProperty = value; }
			}
			public bool _onWriteonlyProperty;

			public bool OnPrivateProperty
			{
				get { return this.PrivateProperty; }
				set { this.PrivateProperty = value; }
			}

			/*********
			** Public methods
			*********/
			public Sword() { }
			public Sword([AlwaysFails] bool value) { }

			public bool OnMethodParameter([AlwaysFails] bool value)
			{
				return value;
			}

			[return: AlwaysFails]
			public bool OnMethodReturnValue(bool value)
			{
				return value;
			}

			[AlwaysFails]
			public bool OnMethod(bool value)
			{
				return value;
			}
		}

		/// <summary>A static class containing annotated code.</summary>
		[DesignedByContract]
		public static class StaticSword
		{
			/*********
			** Properties
			*********/
			[AlwaysFails]
			private static bool PrivateProperty
			{
				get { return StaticSword._privateProperty; }
				set { StaticSword._privateProperty = value; }
			}
			public static bool _privateProperty;


			/*********
			** Accessors
			*********/
			[AlwaysFails]
			public static bool OnProperty
			{
				get { return StaticSword._onProperty; }
				set { StaticSword._onProperty = value; }
			}
			public static bool _onProperty;

			public static bool OnPrivateProperty
			{
				get { return StaticSword.PrivateProperty; }
				set { StaticSword.PrivateProperty = value; }
			}


			/*********
			** Public methods
			*********/
			public static bool OnMethodParameter([AlwaysFails] bool value)
			{
				return value;
			}

			[return: AlwaysFails]
			public static bool OnMethodReturnValue(bool value)
			{
				return value;
			}

			[AlwaysFails]
			public static bool OnMethod(bool value)
			{
				return value;
			}
		}
	}
}