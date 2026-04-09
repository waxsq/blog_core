using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities.Vo.Post
{
    public class PostAddOrEditVo
    {
        public long BlogPostId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public long CategoryId { get; set; }
        public List<BlogTag>? Tags { get; set; }
        public string? TagIds { get; set; }
    }
}
