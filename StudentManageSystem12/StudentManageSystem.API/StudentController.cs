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
    /// 学生API控制器 - 使用过滤器重构版本
    /// </summary>
    [RoutePrefix("api/student")]
    [ActionLogFilter] // 全局操作日志
    [GlobalExceptionFilter] // 全局异常处理
    public class StudentController : ApiController
    {
        private readonly IStudentBll _studentBll;
        private readonly LoginLogBll _loginLogBll;

        public StudentController(IStudentBll studentBll, LoginLogBll loginLogBll)
        {
            _studentBll = studentBll;
            _loginLogBll = loginLogBll;
        }

        /// <summary>
        /// 学生注册接口
        /// </summary>
        [HttpPost]
        [Route("register")]
        [ModelValidationFilter] // 模型验证过滤器
        public IHttpActionResult Register(Student student)
        {
            var result = _studentBll.Register(student);
            return Json(result);
        }

        /// <summary>
        /// 学生登录接口
        /// </summary>
        [HttpPost]
        [Route("checkLogin")]
        [ModelValidationFilter] // 模型验证过滤器
        [LoginLogFilter] // 登录日志过滤器
        public IHttpActionResult CheckLogin(LoginModel loginData)
        {
            // 移除了手动日志记录，由过滤器处理
            var result = _studentBll.CheckLogin(loginData.stuName, loginData.stuPwd);
            return Json(result);
        }

        /// <summary>
        /// 多条件查询学生 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("query")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult QueryStudents(QueryStudentModel queryModel, bool isAdmin = false)
        {
            var result = _studentBll.QueryStudentsByConditions(queryModel, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 按学号查询学生信息
        /// </summary>
        [HttpGet]
        [Route("getById/{stuId}")]
        public IHttpActionResult GetStudentById(int stuId)
        {
            var result = _studentBll.QueryPersonalInfo(stuId);
            return Json(result);
        }

        /// <summary>
        /// 更新学生信息 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("update")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult UpdateStudent(Student student, bool isAdmin = false)
        {
            var result = _studentBll.UpdatePersonalInfo(student);
            return Json(result);
        }

        /// <summary>
        /// 删除学生 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("delete")]
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult DeleteStudent(int stuId, bool isAdmin = false)
        {
            var result = _studentBll.DeleteStudent(stuId, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 获取所有学生 - 需要管理员权限
        /// </summary>
        [HttpGet]
        [Route("getAll")]
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult GetAllStudents(bool isAdmin = false)
        {
            var result = _studentBll.QueryAllStudents(isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 添加学生 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("add")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult AddStudent(Student student, bool isAdmin = false)
        {
            var result = _studentBll.AddStudent(student, isAdmin);
            return Json(result);
        }
    }

    // 登录请求模型（接收前端参数）
    public class LoginModel
    {
        public string stuName { get; set; }
        public string stuPwd { get; set; }
    }



}