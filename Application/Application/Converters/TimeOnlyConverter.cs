using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Kiểm tra loại token của reader
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Invalid token type for TimeOnly conversion. Expected a string.");
            }

            var timeString = reader.GetString();
            return TimeOnly.Parse(timeString);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm"));
        }
    }
}
