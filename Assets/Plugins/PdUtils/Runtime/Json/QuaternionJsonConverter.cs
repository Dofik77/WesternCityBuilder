using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PdUtils.Json
{
	public class QuaternionJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var quaternion = (Quaternion) value;
			var str = quaternion.x + "|" + quaternion.y + "|" + quaternion.z + "|" + quaternion.w;
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
			var w = float.Parse(tmp[3]);
			return new Quaternion(x, y, z, w);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Quaternion);
		}
	}

	public class NullableQuaternionJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var quaternion = (Quaternion?) value;
			if (!quaternion.HasValue)
				return;
			var q = quaternion.Value;
			var str = q.x + "|" + q.y + "|" + q.z + "|" + q.w;
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
			var w = float.Parse(tmp[3]);
			return new Quaternion(x, y, z, w);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Quaternion?);
		}
	}
}