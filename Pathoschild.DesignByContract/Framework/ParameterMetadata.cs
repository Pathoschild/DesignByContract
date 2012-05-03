using System;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework
{
	[Serializable]
	public struct ParameterMetadata
	{
		public ParameterInfo Parameter { get; set; }
		public int Index { get; set; }
		public IParameterContractAnnotation Annotation { get; set; }
	}
}