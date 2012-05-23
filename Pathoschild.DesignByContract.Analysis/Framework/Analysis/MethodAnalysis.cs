using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
	/// <summary>Contains contract metadata about an annotated method or property.</summary>
	[Serializable, DataContract]
	public struct MethodAnalysis
	{
		/*********
		** Accessors
		*********/
		/// <summary>The contract requirements for each method parameter or property setter value.</summary>
		[DataMember]
		public ParameterMetadata[] ParameterPreconditions { get; set; }

		/// <summary>The contract requirements on a method or property return value.</summary>
		[DataMember]
		public ReturnValueMetadata[] ReturnValuePreconditions { get; set; }

		/// <summary>Whether the method has contract annotations to enforce.</summary>
		[IgnoreDataMember]
		public bool HasContract
		{
			get { return this.ParameterPreconditions.Any() || this.ReturnValuePreconditions.Any(); }
		}


		/*********
		** Public methods
		*********/
		/// <summary>Get a human-readable representation of the method analysis.</summary>
		public override string ToString()
		{
			return String.Format(
				"Analysis: {{ HasContract={0}, ParameterPreconditions=[{1}], ReturnValuePreconditions=[{2}] }}",
				this.HasContract,
				String.Join(", ", this.ParameterPreconditions.Select(p => String.Format(@"{{TypeName='{0}', MethodName='{1}', Parameter='{2}', Annotation={3}}}", p.TypeName, p.MethodName, p.Name, p.Annotation.GetType().Name))),
				String.Join(", ", this.ReturnValuePreconditions.Select(p => String.Format(@"{{TypeName='{0}', MethodName='{1}', Annotation={2}}}", p.TypeName, p.MethodName, p.Annotation.GetType().Name)))
			);
		}
	}
}
