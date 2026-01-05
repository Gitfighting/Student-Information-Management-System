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
    /// 管理员权限验证过滤器
    /// </summary>
    public class AdminAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                bool isAdmin = GetIsAdminFromRequest(actionContext);

                if (!isAdmin)
                {
                    var response = new ResultVO
                    {
                        code = 0,
                        message = "需要管理员权限才能执行此操作",
                        data = null
                    };

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, response);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[AdminAuthorizationFilter] 权限验证异常: {ex.Message}");

                var response = new ResultVO
                {
                    code = 0,
                    message = "权限验证失败",
                    data = null
                };

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, response);
            }
        }

        private bool GetIsAdminFromRequest(HttpActionContext actionContext)
        {
            // 1. 从 Action 参数中获取 (POST请求的参数)
            var isAdminParam = actionContext.ActionArguments
                .FirstOrDefault(a => a.Key == "isAdmin").Value as bool?;

            if (isAdminParam == true)
            {
                System.Diagnostics.Trace.WriteLine("[AdminAuth] 从Action参数获取到isAdmin=true");
                return true;
            }

            // 2. 从查询字符串中获取 (GET请求的?isAdmin=true)
            var queryParams = actionContext.Request.GetQueryNameValuePairs();
            var isAdminQuery = queryParams.FirstOrDefault(q => q.Key.Equals("isAdmin", StringComparison.OrdinalIgnoreCase)).Value;

            if (bool.TryParse(isAdminQuery, out bool isAdminFromQuery) && isAdminFromQuery)
            {
                System.Diagnostics.Trace.WriteLine($"[AdminAuth] 从查询字符串获取到isAdmin={isAdminFromQuery}");
                return true;
            }

            System.Diagnostics.Trace.WriteLine("[AdminAuth] 未找到有效的isAdmin参数");
            return false;
        }
    }

}