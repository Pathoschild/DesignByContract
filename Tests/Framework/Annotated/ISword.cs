namespace Pathoschild.DesignByContract.Tests.Framework.Annotated
{
	/// <summary>Represents an annotated test class.</summary>
	public interface ISword
	{
		/*********
		** Accessors
		*********/
		/// <summary>An annotated indexer.</summary>
		[AlwaysFails]
		bool this[bool key] { get; set; }

		/// <summary>An annotated property.</summary>
		[AlwaysFails]
		bool OnProperty { get; set; }

		/// <summary>An annotated property with no setter.</summary>
		[AlwaysFails]
		bool OnReadonlyProperty { get; }

		/// <summary>An annotated property with no getter.</summary>
		[AlwaysFails]
		bool OnWriteonlyProperty { set; }

		/// <summary>A property which wraps an annotated private property.</summary>
		bool OnPrivateProperty { get; set; }


		/*********
		** Methods
		*********/
		/// <summary>A method with an annotated parameter.</summary>
		/// <param name="value">The return value.</param>
		bool OnMethodParameter([AlwaysFails] bool value);

		/// <summary>A method with an annotated return value.</summary>
		/// <param name="value">The return value.</param>
		[return: AlwaysFails]
		bool OnMethodReturnValue(bool value);

		/// <summary>An annotated method.</summary>
		/// <param name="value">The return value.</param>
		[AlwaysFails]
		bool OnMethod(bool value);
	}
}