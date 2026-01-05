using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using StudentManageSystem.Common;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace StudentManageSystem.API.Filters
{
    /// <summary>
    /// 模型验证过滤器 - 统一处理ModelState验证
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                var response = new ResultVO
                {
                    code = 0,
                    message = $"数据格式错误：{string.Join(", ", errors)}",
                    data = null
                };

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }
}