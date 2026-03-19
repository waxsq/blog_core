using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities.Vo.Tag
{
    public class TagAddOrEdit
    {
        public long BlogTagId { get; set; }
        public string TagName { get; set; }
        public int IsValid { get; set; }

        public string Action { get; set; }
    }
}
