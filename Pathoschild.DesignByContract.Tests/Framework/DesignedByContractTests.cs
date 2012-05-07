using System;
using NUnit.Framework;

namespace Pathoschild.DesignByContract.Tests.Framework
{
	/// <summary>Unit tests which ensure <see cref="DesignedByContractAttribute"/> supports all expected scenarios.</summary>
	[TestFixture]
	public class DesignedByContractTests
	{
		/*********
		** Unit tests
		*********/
		/***
		** Constructors
		***/
		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=Sword::.ctor")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=Sword::.ctor")]
		public bool OnConstructorParameter(bool value)
		{
			new Sword(value);
			return value;
		}

		/***
		** Methods (instance)
		***/
		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=Sword::OnMethodParameter")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=Sword::OnMethodParameter")]
		public bool OnMethodParameter(bool value)
		{
			return new Sword().OnMethodParameter(value);
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=Sword::OnMethodReturnValue")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=Sword::OnMethodReturnValue")]
		public bool OnMethodReturnValue(bool value)
		{
			return new Sword().OnMethodReturnValue(value);
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=Sword::OnMethod")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=Sword::OnMethod")]
		public bool OnMethod(bool value)
		{
			return new Sword().OnMethod(value);
		}

		/***
		** Methods (static)
		***/
		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=StaticSword::OnMethodParameter")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=StaticSword::OnMethodParameter")]
		public bool OnStaticMethodParameter(bool value)
		{
			return StaticSword.OnMethodParameter(value);
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=StaticSword::OnMethodReturnValue")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=StaticSword::OnMethodReturnValue")]
		public bool OnStaticMethodReturnValue(bool value)
		{
			return StaticSword.OnMethodReturnValue(value);
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=StaticSword::OnMethod")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=StaticSword::OnMethod")]
		public bool OnStaticMethod(bool value)
		{
			return StaticSword.OnMethod(value);
		}

		/***
		** Properties (instance)
		***/
		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=Sword::get_OnProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=Sword::get_OnProperty")]
		public bool OnProperty_Get(bool value)
		{
			return new Sword { _onProperty = value }.OnProperty;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=Sword::set_OnProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=Sword::set_OnProperty")]
		public bool OnProperty_Set(bool value)
		{
			return new Sword().OnProperty = value;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=Sword::get_PrivateProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=Sword::get_PrivateProperty")]
		public bool OnProperty_PrivateGet(bool value)
		{
			return new Sword { _privateProperty = value }.OnPrivateProperty;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=Sword::set_PrivateProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=Sword::set_PrivateProperty")]
		public bool OnProperty_PrivateSet(bool value)
		{
			return new Sword().OnPrivateProperty = value;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=Sword::get_OnReadonlyProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=Sword::get_OnReadonlyProperty")]
		public bool OnProperty_ReadonlyGet(bool value)
		{
			return new Sword { _onReadonlyProperty = value }.OnReadonlyProperty;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=Sword::set_OnWriteonlyProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=Sword::set_OnWriteonlyProperty")]
		public bool OnProperty_WriteonlyGet(bool value)
		{
			return new Sword().OnWriteonlyProperty = value;
		}

		/***
		** Properties (static)
		***/
		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=StaticSword::get_OnProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=StaticSword::get_OnProperty")]
		public bool OnProperty_StaticGet(bool value)
		{
			StaticSword._onProperty = value;
			return StaticSword.OnProperty;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=StaticSword::set_OnProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=StaticSword::set_OnProperty")]
		public bool OnProperty_StaticSet(bool value)
		{
			return StaticSword.OnProperty = value;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "value=True, friendly=StaticSword::get_PrivateProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "value=False, friendly=StaticSword::get_PrivateProperty")]
		public bool OnProperty_StaticPrivateGet(bool value)
		{
			StaticSword._privateProperty = value;
			return StaticSword.OnPrivateProperty;
		}

		[TestReturnCase(true, typeof(Exception), ExpectedMessage = "parameter=value, value=True, friendly=StaticSword::set_PrivateProperty")]
		[TestReturnCase(false, typeof(Exception), ExpectedMessage = "parameter=value, value=False, friendly=StaticSword::set_PrivateProperty")]
		public bool OnProperty_StaticPrivateSet(bool value)
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
