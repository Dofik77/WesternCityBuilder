using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PdUtils.Json
{
	public class Vector2JsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vector2 = (Vector2) value;
			var str = vector2.x + "|" + vector2.y;
			writer.WriteValue(str);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var str = (string) reader.Value;
			var tmp = str.Split('|');
			var x = float.Parse(tmp[0]);
			var y = float.Parse(tmp[1]);
			return new Vector2(x, y);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2);
		}
	}

	public class NullableVector2JsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vector2 = (Vector2?) value;
			if (!vector2.HasValue)
				return;
			var v = vector2.Value;
			var str = v.x + "|" + v.y;
			writer.WriteValue(str);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var str = (string) reader.Value;
			var tmp = str.Split('|');
			var x = float.Parse(tmp[0]);
			var y = float.Parse(tmp[1]);
			return new Vector2(x, y);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2?);
		}
	}
}