using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Commons
{
    public class ResultBase
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 状态码 (业务状态码，非 HTTP 状态码)
        /// 例如：200=成功, 400=参数错误, 500=系统异常
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息提示 (成功时的提示或错误原因)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间戳 (可选，用于前端调试或防缓存)
        /// </summary>
        public long Timestamp { get; set; }

        public ResultBase()
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

    }
}
