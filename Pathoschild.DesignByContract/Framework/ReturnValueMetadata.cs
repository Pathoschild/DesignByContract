using System;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a return value annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ReturnValueMetadata"/> representation.</remarks>
	[Serializable]
	public struct ReturnValueMetadata
	{
		/*********
		** Accessors
		*********/
		/// <summary>The name of the type.</summary>
		public string TypeName { get; set; }

		/// <summary>The name of the method.</summary>
		public string MethodName { get; set; }

		/// <summary>The contract annotation applied to the return value.</summary>
		public IReturnValuePrecondition Annotation { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="method">The method on which the return value is defined.</param>
		/// <param name="annotation">The contract annotation applied to the return value.</param>
		public ReturnValueMetadata(MemberInfo method, IReturnValuePrecondition annotation)
			: this()
		{
			this.TypeName = method.DeclaringType.Name;
			this.MethodName = method.Name;
			this.Annotation = annotation;
		}
	}
}