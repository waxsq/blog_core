using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;


namespace Blog.Core.Entities.Vo.Tag
{
    public class TagTableQueryVo : PageRequest
    {
        public string? TagName { get; set; }
        public int IsValid { get; set; } = 1; // 默认查询有效数据
    }
}
