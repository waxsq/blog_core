using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Core.Commons
{
    public class LongToStringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 反序列化时：如果前端传的是字符串，尝试解析为 long
            // 反序列化时：如果前端传的是字符串，尝试解析为 long
            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();

                // 1. 检查是否为空或 null
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return 0; // 或者根据业务需求返回默认值
                }

                // 2. 使用 TryParse 安全解析
                if (long.TryParse(stringValue, out long result))
                {
                    return result;
                }

                // 3. 如果字符串确实不是数字（例如传了 "abc"），可以选择抛出异常或返回默认值
                // throw new JsonException($"Unable to convert '{stringValue}' to Int64.");
                return 0;
            }

            // 如果是数字类型，直接读取
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }

            // 兜底处理
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            // 序列化时：强制将 long 写为字符串 "123456..."
            writer.WriteStringValue(value.ToString());
        }
    }
}
