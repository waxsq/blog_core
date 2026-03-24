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
            if (reader.TokenType == JsonTokenType.String)
            {
                return long.Parse(reader.GetString()!);
            }
            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            // 序列化时：强制将 long 写为字符串 "123456..."
            writer.WriteStringValue(value.ToString());
        }
    }
}
