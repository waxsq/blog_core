using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blog.FileStorage.Core
{
    public class LocalFileStrategy : FileUploadStrategy
    {
        private readonly string _rootPath;
        private readonly string _domain;
        public LocalFileStrategy(IWebHostEnvironment environment, string domain = "http://localhost:5263")
        {
            _domain = domain;
            _rootPath = environment.WebRootPath; // 或者配置自定义路径
        }
        protected override string GetAccessUrl(string path)
        {
            return $"{_domain}/{path.Replace("\\", "/")}";
        }

        protected override async Task SaveFileAsync(IFormFile file, string path)
        {
            // 确保目录存在
            var fullPath = Path.Combine(_rootPath, path);
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 写入文件流
            using (var stream = new FileStream(fullPath, FileMode.Create,FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
