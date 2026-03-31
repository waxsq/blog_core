using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities.Vo.Post
{
    public class PostTablePageVo : BlogPost
    {
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}
