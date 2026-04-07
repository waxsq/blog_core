using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities
{
    public class FileResponseResult
    {
        public int Success { get; set; }
        public string Url { get; set; } // 访问路径（URL）
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Message { get; set; }
    }
}
