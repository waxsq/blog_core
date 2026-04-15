using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Core.Commons;

namespace Blog.Core.Entities.Vo.Comment
{
    public class CommentTableQueryVo : PageRequest
    {
        public string? PostTitle { get; set; }
        public int Status { get; set; } = -1;
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        public DateTime? CreateBeginAt { get; set; }
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        public DateTime? CreateEndAt { get; set; }
    }
}
