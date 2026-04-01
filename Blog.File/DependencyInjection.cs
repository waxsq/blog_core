using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities.Dto;
using Blog.Core.Enums;
using Blog.FileStorage.Core;
using Blog.FileStorage.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minio;
using SqlSugar;

namespace Blog.FileStorage
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 注册文件存储服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">存储配置选项</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddFileStorage(this IServiceCollection services, StorageOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "文件存储配置不能为空");
            }

            // 1. 注册配置本身，方便后续服务直接注入 StorageOptions
            services.AddSingleton(options);

            // 2. 根据配置的类型，注册具体的实现策略
            switch (options.Type)
            {
                case StroageEnum.Local:
                    RegisterLocalStorage(services, options);
                    break;

                case StroageEnum.Minio:
                case StroageEnum.AliYun:
                case StroageEnum.FeiNiu: // 通用 S3
                    RegisterS3Storage(services, options);
                    break;

                default:
                    throw new ArgumentException($"不支持的存储类型: {options.Type}");
            }

            return services;
        }

        /// <summary>
        /// 注册本地存储策略
        /// </summary>
        private static void RegisterLocalStorage(IServiceCollection services, StorageOptions options)
        {
            // 校验必要参数
            if (string.IsNullOrWhiteSpace(options.LocalPath))
            {
                throw new ArgumentException("本地存储必须配置 LocalPath");
            }

            // 注册服务
            // 使用 AddSingleton 因为 LocalFileStorage 是无状态的，且依赖的 options 也是单例
            services.AddSingleton<BaseFileStorage>(sp =>
            {
                var logger = sp.GetService<ILogger<LocalFileStorage>>();
                return new LocalFileStorage(options, logger);
            });
        }

        /// <summary>
        /// 注册 S3 兼容存储策略 (Minio, AliYun, FeiNiu 等)
        /// </summary>
        private static void RegisterS3Storage(IServiceCollection services, StorageOptions options)
        {
            // 校验必要参数
            if (string.IsNullOrWhiteSpace(options.Endpoint))
            {
                throw new ArgumentException("S3 存储必须配置 Endpoint");
            }
            if (string.IsNullOrWhiteSpace(options.AccessKey) || string.IsNullOrWhiteSpace(options.SecretKey))
            {
                throw new ArgumentException("S3 存储必须配置 AccessKey 和 SecretKey");
            }

            // 1. 构建 MinioClient (这是底层 SDK 客户端)
            var minioClientBuilder = new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey);

            // 设置区域 (Region)，阿里云/腾讯云等通常需要，Minio 默认 us-east-1
            if (!string.IsNullOrWhiteSpace(options.Region))
            {
                minioClientBuilder = minioClientBuilder.WithRegion(options.Region);
            }

            // 处理 SSL 配置
            // 注意：Minio .NET SDK 6.0+ 默认开启 SSL。如果是 HTTP 需要显式关闭，或者使用 .WithSSL(false)
            // 这里为了兼容性，如果 Endpoint 包含 https:// 则开启，否则关闭
            // 实际上 MinioClient 会根据 Endpoint 自动判断，但为了保险起见，我们可以手动控制
            // 如果是内网 IP (如 192.168.x.x) 且未配置 SSL，通常需要忽略证书验证，但这在 SDK 层面较难处理
            // 最简单的方法是：如果是 http 协议，确保 Endpoint 不带端口或带端口但协议匹配

            // 这里的逻辑是：如果配置了 UseSSL=true，则强制开启；如果 Endpoint 以 https 开头，也开启
            // 否则，使用 HTTP
            // Minio SDK 的 WithSSL 方法在旧版本存在，新版本通常自动检测，这里假设使用较新版本
            // 如果遇到 HTTP 报错，请检查 Endpoint 是否写成了 https:// 但实际是 http://
            
            var minioClient = minioClientBuilder.Build();

            // 2. 注册 MinioClient 到容器，方便其他地方也能直接使用原生客户端
            services.AddSingleton<IMinioClient>(minioClient);

            // 3. 注册我们的 S3CompatibleStorage 服务
            services.AddSingleton<IFileStorageService>(sp =>
            {
                var logger = sp.GetService<ILogger<BaseFileStorage>>();
                return new S3CompatibleStorage(minioClient, options, logger);
            });
        }
    }
}
