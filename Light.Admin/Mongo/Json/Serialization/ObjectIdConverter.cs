using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization
{
    public class ObjectIdConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString();
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }
            if (ObjectId.TryParse(text, out var result) == false)
            {
                throw new FormatException($"“{text}” is an invalid ObjectId format.");
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
