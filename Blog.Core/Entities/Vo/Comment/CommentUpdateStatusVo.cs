using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities.Vo.Comment
{
    public class CommentUpdateStatusVo
    {
        public List<long> Ids {  get; set; }
        public int Status {  get; set; }
    }
}
