using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace Blog.FileStorage.Core
{
    public abstract class FileUploadStrategy
    {
        // --- 模版方法 (核心流程控制) ---
        public async Task<FileResponseResult> UploadAsync(IFormFile file, string subPath)
        {
            var result = new FileResponseResult();

            // 1. 基础校验 (通用逻辑)
            if (file == null || file.Length == 0)
            {
                result.Success = 0;
                result.Message = "文件为空";
                return result;
            }

            // 2. 生成唯一文件名 (通用逻辑，防止重名)
            var fileName = GenerateFileName(file.FileName);
            var fullPath = Path.Combine(subPath, fileName);

            try
            {
                // 3. 执行具体的上传逻辑 (由子类实现)
                await SaveFileAsync(file, fullPath);

                // 4. 构建返回结果 (通用逻辑)
                result.Success = 1;
                result.FileName = fileName;
                result.FileSize = file.Length;
                result.Url = GetAccessUrl(fullPath); // 获取访问链接

                return result;
            }
            catch (Exception ex)
            {
                result.Success = 0;
                result.Message = ex.Message;
                return result;
            }
        }

        // --- 钩子方法/抽象方法 (子类必须实现) ---

        // 具体的保存逻辑（本地IO流、MinIO API等）
        protected abstract Task SaveFileAsync(IFormFile file, string path);

        // 获取访问链接（本地可能是物理路径拼接域名，MinIO则是预签名URL）
        protected abstract string GetAccessUrl(string path);

        // --- 辅助方法 ---
        private string GenerateFileName(string originalName)
        {
            // 简单示例：时间戳+GUID+后缀
            var ext = Path.GetExtension(originalName);
            return $"{DateTime.Now:yyyyMMddHHmmss}_{SnowFlakeSingle.Instance.NextId()}{ext}";
        }
    }
}
