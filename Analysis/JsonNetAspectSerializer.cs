using System;
using System.IO;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Serialization;
using PostSharp.Reflection;

namespace Pathoschild.DesignByContract
{
	/// <summary>Serializes aspect instances at compile-time and deserializes them at runtime using Json.NET.</summary>
	public class JsonNetAspectSerializer : AspectSerializer
	{
		/*********
		** Properties
		*********/
		/// <summary>The Json.NET serializer settings.</summary>
		private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			Formatting = Formatting.None
		};


		/*********
		** Public methods
		*********/
		/// <summary>Serialize a set of aspects into a stream.</summary>
		/// <param name="aspects">The aspects to serialize.</param>
		/// <param name="stream">The stream to which to write.</param>
		/// <param name="metadataEmitter">A metadata emitter for the current module.</param>
		[Obsolete]
		public override void Serialize(IAspect[] aspects, Stream stream, IMetadataEmitter metadataEmitter)
		{
			using (StreamWriter writer = new StreamWriter(stream))
			{
				string serialized = JsonConvert.SerializeObject(aspects, this.Settings);
				writer.Write(serialized);
			}
		}

		/// <summary>Deserialize a set of aspects.</summary>
		/// <param name="stream">The stream from which to read.</param>
		/// <param name="metadataDispenser">Metadata dispenser to be used to resolve serialized metadata references in <paramref name="stream"/>.</param>
		[Obsolete]
		protected override IAspect[] Deserialize(Stream stream, IMetadataDispenser metadataDispenser)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				string serialized = reader.ReadToEnd();
				return JsonConvert.DeserializeObject<IAspect[]>(serialized, this.Settings);
			}
		}
	}
}