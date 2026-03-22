using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Commons
{
    public class EditReponse<T> : ResultBase
    {
        /// <summary>
        /// 返回的数据实体
        /// </summary>
        public T Data { get; set; }
    }

    public class ResultReponse : EditReponse<object>
    {
        public ResultReponse() : base() { }
    }
}
