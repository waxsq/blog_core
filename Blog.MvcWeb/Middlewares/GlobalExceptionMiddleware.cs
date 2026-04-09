using System.DirectoryServices.Protocols;
using System.Text.Json;
using Blog.Core.Commons;
using Blog.Core.Exceptions;
using Blog.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace Blog.MvcWeb.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 如果响应已经开始写入，则无法再修改，直接返回
            if (context.Response.HasStarted)
            {
                return;
            }

            var response = context.Response;
            response.ContentType = "application/json";

            EditReponse<object> result;
            int statusCode;

            // 1. 再次检查是否是业务异常 (以防过滤器未注册或失效)
            if (ex is BusinessException bizEx)
            {
                statusCode = bizEx.Code >= 500 ? 500 : bizEx.Code;
                result = ResultUtil.Fail<object>(bizEx.Message, bizEx.Code);
                _logger.LogWarning("中间件捕获业务异常: {Message}", bizEx.Message);
            }
            // 2. 处理 404 (路由未找到)
            else if (ex is NotImplementedException || context.Response.StatusCode == 404)
            {
                // 注意：路由错误有时不会抛异常，而是直接设状态码。
                // 如果是这里捕获的异常导致的 404
                statusCode = 404;
                result = ResultUtil.Fail<object>("请求的资源不存在", 404);
                _logger.LogWarning("资源未找到: {Path}", context.Request.Path);
            }
            // 3. 处理其他所有未知系统异常 (兜底)
            else
            {
                statusCode = 500;
                // 生产环境不要将 ex.Message 直接返回，防止泄露敏感信息
                result = ResultUtil.Error<object>($"系统内部错误，请联系管理员。{ex.Message}");

                // 记录完整的堆栈跟踪，供开发人员排查
                _logger.LogError(ex, "发生未捕获的系统异常 | Path: {Path} | Method: {Method}",
                    context.Request.Path, context.Request.Method);
            }

            response.StatusCode = statusCode;
            await response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
