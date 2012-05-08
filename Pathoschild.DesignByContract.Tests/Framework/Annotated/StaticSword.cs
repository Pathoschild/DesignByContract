using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathoschild.DesignByContract.Tests.Framework.Annotated
{
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

		[AlwaysFails]
		public static bool OnReadonlyProperty
		{
			get { return StaticSword._onReadonlyProperty; }
		}
		public static bool _onReadonlyProperty;

		[AlwaysFails]
		public static bool OnWriteonlyProperty
		{
			set { StaticSword._onWriteonlyProperty = value; }
		}
		public static bool _onWriteonlyProperty;

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
