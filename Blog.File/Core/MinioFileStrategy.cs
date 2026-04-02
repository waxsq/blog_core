using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;

namespace Blog.FileStorage.Core
{
    public class MinioFileStrategy : FileUploadStrategy
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly string _endpoint;

        public MinioFileStrategy(string endpoint, string accessKey, string secretKey, string bucket)
        {
            _bucketName = bucket;
            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
            _endpoint = endpoint;
        }

        protected override string GetAccessUrl(string path)
        {
            return $"{_endpoint}/{_bucketName}/{path}";
        }

        protected override async Task SaveFileAsync(IFormFile file, string path)
        {
            // MinIO 使用流上传
            using (var stream = file.OpenReadStream())
            {
                var args = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(path) // 路径作为对象名
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length);

                await _minioClient.PutObjectAsync(args);
            }
        }
    }
}
