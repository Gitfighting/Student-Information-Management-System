
using System.Web.Http;
using StudentManageSystem.API.Filters;
using StudentManageSystem.BLL;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.API
{
    /// <summary>
    /// 登录日志API - 使用过滤器重构版本
    /// </summary>
    [RoutePrefix("api/loginLog")]
    [ActionLogFilter] // 全局操作日志
    [GlobalExceptionFilter] // 全局异常处理
    public class LoginLogController : ApiController
    {
        private readonly ILoginLogBll _loginLogBll;

        public LoginLogController(ILoginLogBll loginLogBll)
        {
            _loginLogBll = loginLogBll;
        }

        /// <summary>
        /// 记录登录日志
        /// </summary>
        [HttpPost]
        [Route("add")]
        [NullParameterCheckFilter] // 空参数检查过滤器
        public IHttpActionResult AddLoginLog([FromBody] LoginLog log)
        {
            // 移除了手动空值检查，由过滤器处理
            ResultVO result = _loginLogBll.AddLoginLog(log);
            return Json(result);
        }

        /// <summary>
        /// 查询所有登录日志
        /// </summary>
        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllLoginLogs()
        {
            ResultVO result = _loginLogBll.QueryLoginLogs();
            return Json(result);
        }
    }
}