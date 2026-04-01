using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities.Dto
{
    public class FileInfoDto
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public Stream FileStream { get; set; } // 文件流
        public string FullPath { get; set; }   // 最终访问路径
    }
}
