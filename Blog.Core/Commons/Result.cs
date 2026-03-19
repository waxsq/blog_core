using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Core.Commons
{
    public static class Result
    {
        public static ResultReponse<T> Success<T>(T data, string message = "操作成功", int code = 200)
        {
            return new ResultReponse<T>
            {
                Success = true,
                Code = code,
                Message = message,
                Data = data
            };
        }

        public static ResultReponse Success(string message = "操作成功", int code = 200)
        {
            return new ResultReponse
            {
                Success = true,
                Code = code,
                Message = message,
                Data = null
            };
        }


        /// <summary>
        /// 返回分页数据成功
        /// </summary>
        /// <typeparam name="T">列表项类型</typeparam>
        /// <param name="items">当前页数据列表</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页码 (默认1)</param>
        /// <param name="pageSize">每页条数 (默认10)</param>
        /// <param name="message">提示信息</param>
        /// <param name="code">状态码</param>
        public static ResultReponse<PageModel<T>> SuccessPage<T>(
            List<T> items,
            int totalCount,
            int pageIndex = 1,
            int pageSize = 10,
            string message = "查询成功",
            int code = 200)
        {
            var pageData = new PageModel<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items ?? new List<T>()
            };

            return new ResultReponse<PageModel<T>>
            {
                Success = true,
                Code = code,
                Message = message,
                Data = pageData
            };
        }

        /// <summary>
        /// 重载：直接传入 SqlSugar 的 ISugarPagable 结果 (如果你用 SqlSugar 分页)
        /// 注意：SqlSugar 的 ToPageList 通常返回的是 List<T>，需要单独获取 TotalCount
        /// 此重载适用于你已经有了 List 和 Total 的情况
        /// </summary>
        public static ResultReponse<PageModel<T>> SuccessPage<T>(
            PageModel<T> pageModel,
            string message = "查询成功",
            int code = 200)
        {
            return new ResultReponse<PageModel<T>>
            {
                Success = true,
                Code = code,
                Message = message,
                Data = pageModel
            };
        }

        public static ResultReponse<T> Fail<T>(string message, int code = 400)
        {
            return new ResultReponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default(T)
            };
        }

        public static ResultReponse Fail(string message, int code = 400)
        {
            return new ResultReponse
            {
                Success = false,
                Code = code,
                Message = message,
                Data = null
            };
        }

        public static ResultReponse<T> Unauthorized<T>(string message = "未授权，请登录", int code = 401)
        {
            return new ResultReponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default(T)
            };
        }

        public static ResultReponse<T> Error<T>(string message = "服务器错误", Exception ex = null)
        {
            return Fail<T>($"{message}:{ex.Message}", 500);
        }
    }
}
