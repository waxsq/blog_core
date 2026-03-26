using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Core.Commons;

namespace Blog.Core.Entities.Vo.Post
{
    public class PostTableQueryVo : PageRequest
    {
        public string? Title { get; set; }
        public int Status { get; set; } = -1;
        public int IsFeatured { get; set; } = -1;
        public int IsTop { get; set; } = -1;
        public string? CategoryName { get; set; }
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        public DateTime? PublishedBeginAt { get; set; }
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        public DateTime? PublishedEndAt { get; set; }
    }
}
