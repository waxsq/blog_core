using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Core.Commons
{
    /// <summary>
    /// 内部帮助类：提取核心解析逻辑，避免代码重复和循环依赖
    /// </summary>
    internal static class DateTimeJsonHelper
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        public static DateTime Parse(ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s))
                    throw new JsonException("空字符串无法转换为 DateTime");

                // 尝试精确匹配
                if (DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    return dt;
                }

                // 尝试宽松匹配
                if (DateTime.TryParse(s, out dt))
                {
                    return dt;
                }

                throw new JsonException($"日期格式不正确: {s}");
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var v))
            {
                // 自动识别秒(10位) 或 毫秒(13位)
                if (v > 10000000000L)
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds(v).DateTime;
                }
                return DateTimeOffset.FromUnixTimeSeconds(v).DateTime;
            }

            throw new JsonException($"无法将 JSON 值 ({reader.TokenType}) 转换为 DateTime");
        }

        public static void Write(Utf8JsonWriter writer, DateTime value)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    /// <summary>
    /// 用于非空 DateTime 属性的转换器
    /// 用法: [JsonConverter(typeof(JsonDateTimeConverter))]
    /// </summary>
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                throw new JsonException("无法将 null 转换为非空 DateTime 类型");
            }

            return DateTimeJsonHelper.Parse(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            DateTimeJsonHelper.Write(writer, value);
        }
    }

    /// <summary>
    /// 用于可空 DateTime? 属性的转换器
    /// 专门处理 null 和 空字符串 "" 的情况
    /// </summary>
    public class JsonNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 1. 处理 JSON null
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            // 2. 【关键修复】处理 JSON 空字符串 ""
            // 注意：空字符串的 TokenType 是 String，不是 Null！
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s))
                {
                    return null; // 将空字符串视为 null
                }

                // 如果不是空字符串，尝试正常解析
                if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    return dt;
                }
                if (DateTime.TryParse(s, out dt))
                {
                    return dt;
                }

                throw new JsonException($"日期格式不正确: '{s}'");
            }

            // 3. 处理时间戳 (Number)
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var v))
            {
                return v > 10000000000L
                    ? DateTimeOffset.FromUnixTimeMilliseconds(v).DateTime
                    : DateTimeOffset.FromUnixTimeSeconds(v).DateTime;
            }

            // 其他情况抛出异常
            throw new JsonException($"无法将 JSON 值 ({reader.TokenType}) 转换为 DateTime?");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
