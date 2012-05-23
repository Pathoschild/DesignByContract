using System.IO;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Serialization;

namespace Pathoschild.DesignByContract
{
	/// <summary>Serializes aspect instances at compile-time and deserializes them at runtime using Json.NET.</summary>
	public class JsonNetAspectSerializer : AspectSerializer
	{
		/*********
		** Properties
		*********/
		/// <summary>The Json.NET serializer settings.</summary>
		readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			Formatting = Formatting.None
		};


		/*********
		** Public methods
		*********/
		/// <summary>Serialize a set of aspects.</summary>
		/// <param name="aspects">The aspects to serialize.</param>
		/// <param name="stream">The stream to which to write.</param>
		public override void Serialize(IAspect[] aspects, Stream stream)
		{
			using (StreamWriter writer = new StreamWriter(stream))
			{
				string serialized = JsonConvert.SerializeObject(aspects, this.Settings);
				writer.Write(serialized);
			}
		}

		/// <summary>Deserialize a set of aspects.</summary>
		/// <param name="stream">The stream from which to read.</param>
		public override IAspect[] Deserialize(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				string serialized = reader.ReadToEnd();
				return JsonConvert.DeserializeObject<IAspect[]>(serialized, this.Settings);
			}
		}
	}
}
