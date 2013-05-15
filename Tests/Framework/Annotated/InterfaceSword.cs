namespace Pathoschild.DesignByContract.Tests.Framework.Annotated
{
	/// <summary>A class whose annotations are entirely inherited by its interface.</summary>
	[DesignedByContract]
	public class InterfaceSword : ISword
	{
		/*********
		** Properties
		*********/
		/// <summary>An annotated private property accessible through <see cref="OnPrivateProperty"/>.</summary>
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
		public bool this[bool key]
		{
			get { return this._onIndexer; }
			set { this._onIndexer = value; }
		}
		public bool _onIndexer;

		/// <summary>An annotated property.</summary>
		/// <remarks>The value can be set directly using <see cref="_onProperty"/>.</remarks>
		public bool OnProperty
		{
			get { return this._onProperty; }
			set { this._onProperty = value; }
		}
		public bool _onProperty;

		/// <summary>An annotated property with no setter.</summary>
		/// <remarks>The value can be set directly using <see cref="_onReadonlyProperty"/>.</remarks>
		public bool OnReadonlyProperty
		{
			get { return this._onReadonlyProperty; }
		}
		public bool _onReadonlyProperty;

		/// <summary>An annotated property with no getter.</summary>
		public bool OnWriteonlyProperty
		{
			set { this._onWriteonlyProperty = value; }
		}
		private bool _onWriteonlyProperty;

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
		/// <summary>A method with an annotated parameter.</summary>
		/// <param name="value">The return value.</param>
		public bool OnMethodParameter(bool value)
		{
			return value;
		}

		/// <summary>A method with an annotated return value.</summary>
		/// <param name="value">The return value.</param>
		public bool OnMethodReturnValue(bool value)
		{
			return value;
		}

		/// <summary>An annotated method.</summary>
		/// <param name="value">The return value.</param>
		public bool OnMethod(bool value)
		{
			return value;
		}
	}
}