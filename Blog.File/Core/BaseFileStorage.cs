using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities.Dto;
using SqlSugar;

namespace Blog.FileStorage.Core
{
    public abstract class BaseFileStorage
    {
        /// <summary>
        /// 模板方法：定义上传的通用流程
        /// 1. 校验参数
        /// 2. 生成存储路径
        /// 3. 执行具体上传 (钩子方法)
        /// 4. 记录日志/清理
        /// </summary>
        public async Task<string> UploadAsync(FileInfoDto fileModel)
        {
            // --- 1. 通用前置处理 ---
            ValidateFile(fileModel);

            // 生成唯一的文件路径 (例如：/uploads/2026/04/guid.jpg)
            var objectKey = GenerateObjectKey(fileModel);

            try
            {
                // --- 2. 核心业务逻辑 (由子类实现) ---
                var url = await ExecuteUploadAsync(fileModel, objectKey);

                // --- 3. 通用后置处理 ---
                OnUploadSuccess(fileModel, url);

                return url;
            }
            catch (Exception ex)
            {
                OnUploadFailure(fileModel, ex);
                throw; // 抛出异常让调用者处理
            }
        }

        // --- 抽象方法 (具体策略必须实现) ---
        protected abstract Task<string> ExecuteUploadAsync(FileInfoDto fileDto, string objectKey);
        public abstract Task<Stream> ExecuteDownloadAsync(string objectKey);
        public abstract Task<bool> ExecuteDeleteAsync(string objectKey);

        // --- 通用辅助方法 (模板的一部分) ---

        protected void ValidateFile(FileInfoDto model)
        {
            if (model == null || model.FileStream == null)
                throw new ArgumentException("文件流不能为空");

            // 可以在这里统一限制文件大小，例如 10MB
            if (model.FileSize > 10 * 1024 * 1024)
                throw new InvalidOperationException("文件大小超过限制");
        }

        protected string GenerateObjectKey(FileInfoDto model)
        {
            // 统一路径策略：按日期分文件夹
            var date = DateTime.Now.ToString("yyyy/MM/dd");
            var guid = SnowFlakeSingle.instance.NextId();
            return $"{date}/{guid}{model.FileExtension}";
        }

        protected void OnUploadSuccess(FileInfoDto model, string url)
        {
            // 统一日志记录
            Console.WriteLine($"[Success] 文件 {model.FileName} 已上传至 {url}");
        }

        protected void OnUploadFailure(FileInfoDto model, Exception ex)
        {
            Console.WriteLine($"[Error] 文件 {model.FileName} 上传失败: {ex.Message}");
        }
    }
}
