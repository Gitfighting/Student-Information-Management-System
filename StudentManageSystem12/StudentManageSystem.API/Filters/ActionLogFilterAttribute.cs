using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace StudentManageSystem.API.Filters
{
    /// <summary>
    /// 操作日志记录过滤器
    /// </summary>
    public class ActionLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionContext.ActionDescriptor.ActionName;

            System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] 开始执行: {controllerName}.{actionName}");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] 执行完成: {controllerName}.{actionName}");
        }
    }
}