using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentManageSystem.API.Filters;
using StudentManageSystem.BLL;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.API
{
    /// <summary>
    /// 管理员API控制器 
    /// </summary>
    [RoutePrefix("api/admin")]
    [ActionLogFilter] // 全局操作日志
    [GlobalExceptionFilter] // 全局异常处理
    public class AdminController : ApiController
    {
        private readonly IAdminBll _adminBll;
        private readonly ILoginLogBll _loginLogBll;

        public AdminController(IAdminBll adminBll, ILoginLogBll loginLogBll)
        {
            _adminBll = adminBll;
            _loginLogBll = loginLogBll;
        }

        /// <summary>
        /// 管理员登录接口 - 使用过滤器处理验证和日志
        /// </summary>
        [HttpPost]
        [Route("checkLogin")]
        [ModelValidationFilter] // 模型验证过滤器
        [LoginLogFilter] // 登录日志过滤器
        public IHttpActionResult CheckAdminLogin(AdminLoginModel loginData)
        {
            // 移除了手动验证和日志记录，由过滤器处理
            var result = _adminBll.CheckAdminLogin(loginData.adminName, loginData.adminPwd);
            return Json(result);
        }
    }

    // 4. 登录请求模型
    public class AdminLoginModel
    {
        public string adminName { get; set; }
        public string adminPwd { get; set; }
    }
}

