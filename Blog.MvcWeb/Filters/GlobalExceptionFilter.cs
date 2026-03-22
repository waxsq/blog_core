using Blog.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.MvcWeb.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = ResultUtil.Error<object>("系统繁忙，请稍后重试", context.Exception);
            context.Result = new JsonResult(result);
            context.ExceptionHandled = true; // 标记异常已处理
        }
    }
}
