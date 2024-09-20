using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters
{

    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // Bắt đầu đọc đối tượng
                reader.Read(); // Bắt đầu object

                // Đọc key 'hour'
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "hour")
                {
                    throw new JsonException("Expected 'hour' property.");
                }

                // Đọc giá trị giờ
                reader.Read();
                if (reader.TokenType != JsonTokenType.Number)
                {
                    throw new JsonException("Expected hour to be a number.");
                }
                int hour = reader.GetInt32();

                // Đọc key 'minute'
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "minute")
                {
                    throw new JsonException("Expected 'minute' property.");
                }

                // Đọc giá trị phút
                reader.Read();
                if (reader.TokenType != JsonTokenType.Number)
                {
                    throw new JsonException("Expected minute to be a number.");
                }
                int minute = reader.GetInt32();

                // Kết thúc object
                reader.Read();
                if (reader.TokenType != JsonTokenType.EndObject)
                {
                    throw new JsonException("Expected end of object.");
                }

                return new TimeOnly(hour, minute);
            }

            throw new JsonException("Expected an object with 'hour' and 'minute' properties.");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("hour", value.Hour);
            writer.WriteNumber("minute", value.Minute);
            writer.WriteEndObject();
        }
    }



}
