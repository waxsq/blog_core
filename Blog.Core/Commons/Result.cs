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
