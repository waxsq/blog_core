using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Exceptions
{
    public class BusinessException : Exception
    {
        /// <summary>
        /// 业务错误码 (例如: 1001, USER_NOT_FOUND)
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 额外的数据 payload (可选)
        /// </summary>
        public object? Data { get; }

        /// <summary>
        /// HTTP 状态码 (默认 500，可在构造函数中指定)
        /// </summary>
        public int HttpStatusCode { get; }

        /// <summary>
        /// 基础构造函数
        /// </summary>
        /// <param name="message">用户可见的错误消息</param>
        /// <param name="code">业务错误码 (默认 -1)</param>
        /// <param name="httpStatusCode">HTTP 状态码 (默认 500)</param>
        /// <param name="innerException">内部原始异常 (用于日志记录)</param>
        public BusinessException(string message, int code = -1, int httpStatusCode = 500, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
            Data = null;
        }

        /// <summary>
        /// 带额外数据的构造函数
        /// </summary>
        public BusinessException(string message, object data, int code = -1, int httpStatusCode = 500, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
            Data = data;
        }
    }
}
