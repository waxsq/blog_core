using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities.Dto;

namespace Blog.FileStorage.Core
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(FileInfoDto fileModel);
        Task<Stream> DownloadAsync(string objectKey);
        Task<bool> DeleteAsync(string objectKey);
        // 可以扩展分片上传接口
        Task<string> InitMultipartUpload(string fileName);
    }
}
