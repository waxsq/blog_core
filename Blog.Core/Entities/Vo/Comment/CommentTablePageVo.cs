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
    public class CommentTablePageVo
    {
        public long BlogCommentId { get; set; }
        public long PostId { get; set; }
        public string PostTitle { get; set; }
        public string CreateByName { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreateAt { get; set; }
        public int Status { get; set; }
        public string? Address { get; set; }
    }
}
