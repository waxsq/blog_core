using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities.Dto;
using Blog.FileStorage.Core;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Blog.FileStorage.Strategies
{
    public class S3CompatibleStorage : BaseFileStorage
    {
        private readonly IMinioClient _minioClient;
        private readonly StorageOptions _options;
        private readonly ILogger<S3CompatibleStorage> _logger;

        // 构造函数注入 MinioClient 和配置
        public S3CompatibleStorage(IMinioClient minioClient, StorageOptions options, ILogger<S3CompatibleStorage> logger)
        {
            _minioClient = minioClient;
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// 核心上传逻辑
        /// </summary>
        protected override async Task<string> ExecuteUploadAsync(FileInfoDto fileModel, string objectKey)
        {
            // 1. 检查并创建 Bucket (如果配置了自动创建)
            await EnsureBucketExistsAsync();

            // 2. 重置流位置 (防止之前读取过导致位置不在开头)
            if (fileModel.FileStream.CanSeek)
            {
                fileModel.FileStream.Position = 0;
            }

            try
            {
                // 3. 调用 Minio SDK 上传
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_options.BucketName)
                    .WithObject(objectKey)
                    .WithStreamData(fileModel.FileStream)
                    .WithObjectSize(fileModel.FileStream.Length)
                    .WithContentType(fileModel.ContentType);

                await _minioClient.PutObjectAsync(putObjectArgs);

                _logger.LogDebug("文件上传成功: {Bucket}/{Object}", _options.BucketName, objectKey);

                // 4. 返回访问 URL
                // 策略：如果有自定义域名配置则使用自定义域名，否则拼接 Endpoint
                return GetFileAccessUrl(objectKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "S3 上传失败: {Bucket}/{Object}", _options.BucketName, objectKey);
                throw;
            }
        }

        /// <summary>
        /// 核心下载逻辑
        /// 返回文件流，由调用方负责读取和释放
        /// </summary>
        public override async Task<Stream> ExecuteDownloadAsync(string objectKey)
        {
            var memoryStream = new MemoryStream();

            try
            {
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(_options.BucketName)
                    .WithObject(objectKey)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);

                // 复制完后，重置位置到开头
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "S3 下载失败: {Bucket}/{Object}", _options.BucketName, objectKey);
                memoryStream.Dispose(); // 失败则释放内存
                throw;
            }
        }

        /// <summary>
        /// 核心删除逻辑
        /// </summary>
        public override async Task<bool> ExecuteDeleteAsync(string objectKey)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_options.BucketName)
                    .WithObject(objectKey);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                _logger.LogDebug("文件删除成功: {Bucket}/{Object}", _options.BucketName, objectKey);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "S3 删除失败: {Bucket}/{Object}", _options.BucketName, objectKey);
                throw;
            }
        }

        #region 私有辅助方法

        /// <summary>
        /// 确保 Bucket 存在
        /// </summary>
        private async Task EnsureBucketExistsAsync()
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(_options.BucketName);
            bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);

            if (!found)
            {
                _logger.LogInformation("Bucket {Bucket} 不存在，正在创建...", _options.BucketName);
                var makeBucketArgs = new MakeBucketArgs().WithBucket(_options.BucketName);

                // 如果是阿里云等需要指定 Region 的服务，可以在这里加上 .WithLocation(_options.Region)
                // Minio 默认通常不需要指定，或者指定为 us-east-1

                await _minioClient.MakeBucketAsync(makeBucketArgs);
            }
        }

        /// <summary>
        /// 生成文件访问 URL
        /// 注意：生产环境建议配置 CDN 域名，这里仅作演示逻辑
        /// </summary>
        private string GetFileAccessUrl(string objectKey)
        {
            // 方案 A: 如果你有自定义域名配置在 StorageOptions 中
            // if (!string.IsNullOrEmpty(_options.CustomDomain)) 
            // {
            //     return $"https://{_options.CustomDomain}/{objectKey}";
            // }

            // 方案 B: 直接拼接 Endpoint (简单粗暴，但可能有跨域或签名问题)
            // 注意：MinioClient 的 Endpoint 可能包含端口，需要处理 http/https 前缀
            string endpoint = _options.Endpoint;
            if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
            {
                endpoint = (_options.UseSSL ? "https://" : "http://") + endpoint;
            }

            return $"{endpoint}/{_options.BucketName}/{objectKey}";
        }

        #endregion
    }
}
