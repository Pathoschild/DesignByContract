using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pathoschild.DesignByContract.Framework
{
	/// <summary>Represents a return value annotated by a single contract annotation.</summary>
	/// <remarks>If the parameter has multiple annotations, each annotation will have its own <see cref="ReturnValueMetadata"/> representation.</remarks>
	[Serializable, DataContract]
	public struct ReturnValueMetadata
	{
		/*********
		** Accessors
		*********/
		/// <summary>The name of the type.</summary>
		[DataMember]
		public string TypeName { get; set; }

        /// <summary>
        /// The full name of the returned value's type
        /// </summary>
        [DataMember]
        public string ReturnFullTypeName { get; set; }

		/// <summary>The name of the method.</summary>
		[DataMember]
		public string MethodName { get; set; }

		/// <summary>The contract annotation applied to the return value.</summary>
		[DataMember]
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
            this.ReturnFullTypeName = GetReturnType(method).FullName;
			this.MethodName = method.Name;
			this.Annotation = annotation;
		}

        private static Type GetReturnType(MemberInfo method)
        {
            if (method is MethodInfo)
                return (method as MethodInfo).ReturnType;
            if (method is PropertyInfo)
                return (method as PropertyInfo).PropertyType;
            if (method is FieldInfo)
                return (method as FieldInfo).FieldType;
            throw new InvalidOperationException();
        }
	}
}