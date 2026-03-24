using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Utils
{
    public static class ResultUtil
    {
        public static EditReponse<T> Success<T>(T data, string message = "操作成功", int code = 200)
        {
            return new EditReponse<T>
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
        /// @param datas 当前页的数据列表
        /// @param pageRequest 分页请求参数，包含页码和页大小
        /// @param message 可选的成功消息，默认为 "查询成功"
        /// @param code 可选的状态码，默认为 200
        /// @return 包含分页数据和分页信息的 PageReponse 对象
        /// </summary>
        public static PageReponse<T> SuccessPage<T>(
            PageReponse<T> pageReponse,
            string message = "查询成功",
            int code = 200)
        {
            return new PageReponse<T>
            {
                Success = true,
                Code = code,
                Datas = pageReponse.Datas,
                Message = message,
                PageIndex = pageReponse.PageIndex,
                PageSize = pageReponse.PageSize,
                TotalCount = pageReponse.TotalCount
            };
        }

        public static EditReponse<T> Fail<T>(string message, int code = 400)
        {
            return new EditReponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default
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

        public static EditReponse<T> Unauthorized<T>(string message = "未授权，请登录", int code = 401)
        {
            return new EditReponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default
            };
        }

        public static EditReponse<T> Error<T>(string message = "服务器错误", Exception ex = null)
        {
            return Fail<T>($"{message}:{ex.Message}", 500);
        }
    }
}
