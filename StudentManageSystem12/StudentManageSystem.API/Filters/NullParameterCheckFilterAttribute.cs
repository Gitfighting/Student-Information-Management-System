using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using StudentManageSystem.Common;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net.Http;

namespace StudentManageSystem.API.Filters
{
    /// <summary>
    /// 空参数检查过滤器
    /// </summary>
    public class NullParameterCheckFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var fromBodyParams = actionContext.ActionArguments
                .Where(arg => arg.Value != null &&
                             actionContext.ActionDescriptor.GetParameters()
                             .Any(p => p.ParameterName == arg.Key &&
                                  p.GetCustomAttributes<FromBodyAttribute>().Any()))
                .ToList();
            // 检查 Action 参数 
            foreach (var param in fromBodyParams)
            {
                if (param.Value == null)
                {
                    var response = new ResultVO
                    {
                        code = 0,
                        message = "请求参数为空，请检查前端JSON格式",
                        data = null
                    };

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, response);
                    return;
                }
            }
        }
    }
}