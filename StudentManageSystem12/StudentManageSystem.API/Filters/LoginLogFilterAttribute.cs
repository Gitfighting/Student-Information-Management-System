using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using StudentManageSystem.Common;
using StudentManageSystem.Model;
using System.Web.Http.Filters;
using StudentManageSystem.BLL;

namespace StudentManageSystem.API.Filters
{
    /// <summary>
    /// 登录日志记录过滤器 - 专门用于登录操作
    /// </summary>
    public class LoginLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                
                // 1. 获取 Action 返回结果
                var response = actionExecutedContext.Response?.Content as ObjectContent;
                var result = response?.Value as ResultVO;

                if (result != null)
                {
                    var controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.
                        ControllerName;
                    var userType = controllerName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "管理员" : "学生";
                    // 2. 构造日志实体
                    var log = new LoginLog
                    {
                        loginTime = DateTime.Now,
                        userType = userType,
                        operationType = result.code == 2 ? "登录成功" : "登录失败",
                        userId = GetUserId(result, userType)
                    };
                    // 3. 通过反射获取 BLL 服务并写入
                    var controller = actionExecutedContext.ActionContext.ControllerContext.Controller;
                    var loginLogBll = GetLoginLogBll(controller);
                    ((ILoginLogBll)loginLogBll).AddLoginLog(log);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[LoginLogFilter] 记录日志异常: {ex.Message}");
            }
        }

        private ILoginLogBll GetLoginLogBll(object controller)
        {
            // 修复：返回强类型接口而不是 object
            var loginLogField = controller.GetType().GetField("_loginLogBll",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return loginLogField?.GetValue(controller) as ILoginLogBll;
        }

        private string GetUserId(ResultVO result, string userType)
        {
            if (result.code != 2 || result.data == null) return "-";

            if (userType == "学生" && result.data is Student student)
                return student.stuId.ToString();

            if (userType == "管理员" && result.data is Admin admin)
                return admin.AdminId.ToString();

            return "-";
        }
    }
}