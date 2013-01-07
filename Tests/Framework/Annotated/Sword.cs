namespace Pathoschild.DesignByContract.Tests.Framework.Annotated
{
	/// <summary>A class containing annotated code.</summary>
	[DesignedByContract]
	public class Sword
	{
		/*********
		** Properties
		*********/
		/// <summary>An annotated private property accessible through <see cref="OnPrivateProperty"/>.</summary>
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
		/// <summary>An annotated indexer.</summary>
		/// <remarks>The value can be set directly using <see cref="_onIndexer"/>.</remarks>
		[AlwaysFails]
		public bool this[bool key]
		{
			get { return this._onIndexer; }
			set { this._onIndexer = value; }
		}
		public bool _onIndexer;

		/// <summary>An annotated property.</summary>
		/// <remarks>The value can be set directly using <see cref="_onProperty"/>.</remarks>
		[AlwaysFails]
		public bool OnProperty
		{
			get { return this._onProperty; }
			set { this._onProperty = value; }
		}
		public bool _onProperty;

		/// <summary>An annotated property with no setter.</summary>
		/// <remarks>The value can be set directly using <see cref="_onReadonlyProperty"/>.</remarks>
		[AlwaysFails]
		public bool OnReadonlyProperty
		{
			get { return this._onReadonlyProperty; }
		}
		public bool _onReadonlyProperty;

		/// <summary>An annotated property with no getter.</summary>
		[AlwaysFails]
		public bool OnWriteonlyProperty
		{
			set { this._onWriteonlyProperty = value; }
		}
		public bool _onWriteonlyProperty;

		/// <summary>A property which wraps an annotated private property.</summary>
		/// <remarks>The value can be set directly using <see cref="_privateProperty"/>.</remarks>
		public bool OnPrivateProperty
		{
			get { return this.PrivateProperty; }
			set { this.PrivateProperty = value; }
		}


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		public Sword() { }

		/// <summary>Construct an instance with an annotated parameter.</summary>
		/// <param name="value">The return value.</param>
		public Sword([AlwaysFails] bool value) { }

		/// <summary>A method with an annotated parameter.</summary>
		/// <param name="value">The return value.</param>
		public bool OnMethodParameter([AlwaysFails] bool value)
		{
			return value;
		}

		/// <summary>A method with an annotated return value.</summary>
		/// <param name="value">The return value.</param>
		[return: AlwaysFails]
		public bool OnMethodReturnValue(bool value)
		{
			return value;
		}

		/// <summary>An annotated method.</summary>
		/// <param name="value">The return value.</param>
		[AlwaysFails]
		public bool OnMethod(bool value)
		{
			return value;
		}
	}
}
