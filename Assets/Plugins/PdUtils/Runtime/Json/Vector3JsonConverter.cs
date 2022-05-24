using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PdUtils.Json
{
	public class Vector3JsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vector3 = (Vector3) value;
			var str = vector3.x + "|" + vector3.y + "|" + vector3.z;
			writer.WriteValue(str);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var str = (string) reader.Value;
			var tmp = str.Split('|');
			var x = float.Parse(tmp[0]);
			var y = float.Parse(tmp[1]);
			var z = float.Parse(tmp[2]);
			return new Vector3(x, y, z);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector3);
		}
	}

	public class NullableVector3JsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vector3 = (Vector3?) value;
			if (!vector3.HasValue)
				return;
			var v = vector3.Value;
			var str = v.x + "|" + v.y + "|" + v.z;
			writer.WriteValue(str);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var str = (string) reader.Value;
			var tmp = str.Split('|');
			var x = float.Parse(tmp[0]);
			var y = float.Parse(tmp[1]);
			var z = float.Parse(tmp[2]);
			return new Vector3(x, y, z);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector3?);
		}
	}
}