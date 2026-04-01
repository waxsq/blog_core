using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Enums;

namespace Blog.Core.Entities.Dto
{
    public class StorageOptions
    {
        public StroageEnum Type { get; set; }

        // 本地存储配置
        public string LocalPath { get; set; }
        public string LocalUrlPrefix { get; set; } // 如 "/uploads"

        // S3/Minio/阿里云/飞牛 通用配置
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public bool UseSSL { get; set; } = false;
        public string Region { get; set; } = "cn-east-1";
    }
}
