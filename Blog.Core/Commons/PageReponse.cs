using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Commons
{
    /// <summary>
    /// 分页数据模型
    /// </summary>
    /// <typeparam name="T">列表项的类型</typeparam>
    public class PageReponse<T> : ResultBase
    {
        /// <summary>
        /// 当前页码 (从 1 开始)
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; } = 0;


        /// <summary>
        /// 当前页的数据列表
        /// </summary>
        public IEnumerable<T> Datas { get; set; } = new List<T>();
    }
}
