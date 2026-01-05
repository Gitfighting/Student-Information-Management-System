using System;
using System.Web.Http;
using StudentManageSystem.API.Filters;
using StudentManageSystem.BLL;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.API
{
    [RoutePrefix("api/course")]
    [ActionLogFilter] // 全局操作日志
    [GlobalExceptionFilter] // 全局异常处理
    public class CourseController : ApiController
    {
        private readonly ICourseBll _courseBll;

        public CourseController(ICourseBll courseBll)
        {
            _courseBll = courseBll;
        }

        /// <summary>
        /// 添加课程 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("add")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult AddCourse(Course course, bool isAdmin = false)
        {
            var result = _courseBll.AddCourse(course, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 删除课程 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("delete")]
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult DeleteCourse(int courseId, bool isAdmin = false)
        {
            var result = _courseBll.DeleteCourse(courseId, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 更新课程 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("update")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult UpdateCourse(Course course, bool isAdmin = false)
        {
            var result = _courseBll.UpdateCourse(course, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 多条件查询课程
        /// </summary>
        [HttpPost]
        [Route("query")]
        [ModelValidationFilter] // 模型验证过滤器
        [NullParameterCheckFilter] // 空参数检查过滤器
        public IHttpActionResult QueryCourses([FromBody] QueryCourseModel queryModel)
        {
            // 移除了手动异常处理和验证，由过滤器处理
            var result = _courseBll.QueryCourses(queryModel);
            return Json(result);
        }

        /// <summary>
        /// 根据ID查询课程
        /// </summary>
        [HttpGet]
        [Route("getById/{courseId}")]
        public IHttpActionResult GetCourseById(int courseId)
        {
            var result = _courseBll.GetCourseById(courseId);
            return Json(result);
        }

        /// <summary>
        /// 查询所有课程
        /// </summary>
        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllCourses()
        {
            var result = _courseBll.GetAllCourses();
            return Json(result);
        }
    }
}