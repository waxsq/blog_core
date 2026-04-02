using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities
{
    public class FileResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; } // 访问路径（URL）
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ErrorMessage { get; set; }
    }
}
