using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Core.Commons
{
    // 非可空 DateTime
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    return dt;
                }
                if (DateTime.TryParse(s, out dt)) return dt;
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var v))
            {
                return DateTimeOffset.FromUnixTimeSeconds(v).DateTime;
            }

            throw new JsonException($"无法将 JSON 值转换为 DateTime: {reader.GetString()}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    // 可空 DateTime?
    public class JsonNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s)) return null;
                if (DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    return dt;
                }
                if (DateTime.TryParse(s, out dt)) return dt;
            }
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var v))
            {
                return DateTimeOffset.FromUnixTimeSeconds(v).DateTime;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(Format));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
