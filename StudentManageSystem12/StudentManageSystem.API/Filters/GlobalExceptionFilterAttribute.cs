using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using StudentManageSystem.Common;
using System.Web.Http.Filters;
using System.Net.Http;

namespace StudentManageSystem.API.Filters
{
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // 构造标准错误响应
            var response = new ResultVO
            {
                code = -1,
                message = $"服务器内部错误: {actionExecutedContext.Exception.Message}",
                data = null
            };

            // 记录异常日志到 Trace
            System.Diagnostics.Trace.WriteLine($"[异常] {actionExecutedContext.Exception}");

            // 重写 Response，状态码设为 500
            actionExecutedContext.Response = actionExecutedContext.Request
                .CreateResponse(HttpStatusCode.InternalServerError, response);
        }
    }
}