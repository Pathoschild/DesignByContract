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
		/// <summary>The name of the class.</summary>
		[DataMember]
		public string TypeName { get; set; }

		/// <summary>The name of the method.</summary>
		[DataMember]
		public string MethodName { get; set; }

		/// <summary>The fully-qualified name of the return type.</summary>
		[DataMember]
		public string ReturnTypeQualifiedName { get; set; }

		/// <summary>Whether the return type is not known at compile-time. This occurs when the type is generic and the type isn't specified in code.</summary>
		public bool ReturnTypeIsUnknown { get; set; }

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
			Type returnType = this.GetReturnType(method);
			this.TypeName = method.DeclaringType.Name;
			this.ReturnTypeQualifiedName = returnType.AssemblyQualifiedName;
			this.ReturnTypeIsUnknown = returnType.AssemblyQualifiedName == null;
			this.MethodName = method.Name;
			this.Annotation = annotation;
		}

		/// <summary>Get the type of the return value.</summary>
		/// <param name="method">The method.</param>
		/// <exception cref="InvalidOperationException">Cannot determine the return type for this method.</exception>
		[CanBeNull]
		private Type GetReturnType(MemberInfo method)
		{
			if (method is MethodInfo)
				return (method as MethodInfo).ReturnType;
			if (method is PropertyInfo)
				return (method as PropertyInfo).PropertyType;
			if (method is FieldInfo)
				return (method as FieldInfo).FieldType;

			throw new InvalidOperationException(String.Format("Method analysis failed for {0}::{1}: could not determine return type for method of type {2}.", method.DeclaringType != null ? method.DeclaringType.Name : "<null>", method.Name, method.GetType().Name));
		}
	}
}