using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities.Dto;
using Microsoft.Extensions.Logging;

namespace Blog.FileStorage.Core
{
    public class LocalFileStorage : BaseFileStorage
    {
        private readonly StorageOptions _options;
        private readonly ILogger<LocalFileStorage> _logger;

        // 构造函数注入配置和日志
        public LocalFileStorage(StorageOptions options, ILogger<LocalFileStorage> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// 核心上传逻辑 (模板方法调用此方法)
        /// </summary>
        protected override async Task<string> ExecuteUploadAsync(FileInfoDto fileModel, string objectKey)
        {
            // 1. 组合物理路径 (RootPath + 相对路径)
            // 注意：将 URL 格式的路径分隔符 "/" 替换为系统路径分隔符
            var relativePath = objectKey.Replace("/", Path.DirectorySeparatorChar.ToString());
            var physicalPath = Path.Combine(_options.LocalPath, relativePath);

            // 2. 确保目录存在
            var directory = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                _logger.LogDebug("创建目录: {Directory}", directory);
            }

            // 3. 写入文件
            // 使用 FileStream 异步写入，避免阻塞线程
            using (var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                // 确保输入流的位置在开头
                if (fileModel.FileStream.CanSeek)
                {
                    fileModel.FileStream.Position = 0;
                }

                await fileModel.FileStream.CopyToAsync(stream);
            }

            // 4. 生成访问 URL
            // 格式：UrlPrefix + "/" + ObjectKey
            // 例如：/uploads/2026/04/01/abc.png
            var url = $"{_options.LocalUrlPrefix.TrimEnd('/')}/{objectKey}";

            return url;
        }

        /// <summary>
        /// 核心下载逻辑
        /// </summary>
        public override async Task<Stream> ExecuteDownloadAsync(string objectKey)
        {
            var relativePath = objectKey.Replace("/", Path.DirectorySeparatorChar.ToString());
            var physicalPath = Path.Combine(_options.LocalPath, relativePath);

            if (!File.Exists(physicalPath))
            {
                _logger.LogWarning("文件不存在: {Path}", physicalPath);
                throw new FileNotFoundException($"文件不存在: {objectKey}", physicalPath);
            }

            // 返回一个只读的文件流
            // 注意：这里不要立即 using 掉 stream，因为调用方需要读取它
            return new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// 核心删除逻辑
        /// </summary>
        public override Task<bool> ExecuteDeleteAsync(string objectKey)
        {
            var relativePath = objectKey.Replace("/", Path.DirectorySeparatorChar.ToString());
            var physicalPath = Path.Combine(_options.LocalPath, relativePath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                _logger.LogDebug("删除文件: {Path}", physicalPath);
            }

            return Task.FromResult(true);
        }
    }
}
