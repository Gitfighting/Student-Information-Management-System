using System;
using System.Web.Http;
using StudentManageSystem.API.Filters;
using StudentManageSystem.BLL;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.API
{
    [RoutePrefix("api/score")]
    [ActionLogFilter] // 全局操作日志
    [GlobalExceptionFilter] // 全局异常处理
    public class ScoreController : ApiController
    {
        private readonly IScoreBll _scoreBll;

        public ScoreController(IScoreBll scoreBll)
        {
            _scoreBll = scoreBll;
        }

        /// <summary>
        /// 添加成绩 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("add")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult AddScore(Score score, bool isAdmin = false)
        {
            var result = _scoreBll.AddScore(score, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 删除成绩 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("delete")]
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult DeleteScore(int scoreId, bool isAdmin = false)
        {
            var result = _scoreBll.DeleteScore(scoreId, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 更新成绩 - 需要管理员权限
        /// </summary>
        [HttpPost]
        [Route("update")]
        [ModelValidationFilter] // 模型验证过滤器
        [AdminAuthorizationFilter] // 管理员权限验证
        public IHttpActionResult UpdateScore(Score score, bool isAdmin = false)
        {
            var result = _scoreBll.UpdateScore(score, isAdmin);
            return Json(result);
        }

        /// <summary>
        /// 多条件查询成绩
        /// </summary>
        [HttpPost]
        [Route("query")]
        [ModelValidationFilter] // 模型验证过滤器
        [NullParameterCheckFilter] // 空参数检查过滤器
        public IHttpActionResult QueryScores([FromBody] QueryScoreModel queryModel)
        {
            // 移除了手动异常处理和验证，由过滤器处理
            var result = _scoreBll.QueryScores(queryModel);
            return Json(result);
        }

        /// <summary>
        /// 根据ID查询成绩
        /// </summary>
        [HttpGet]
        [Route("getById/{scoreId}")]
        public IHttpActionResult GetScoreById(int scoreId)
        {
            var result = _scoreBll.GetScoreById(scoreId);
            return Json(result);
        }

        /// <summary>
        /// 获取所有成绩
        /// </summary>
        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllScores()
        {
            var result = _scoreBll.GetAllScores();
            return Json(result);
        }
    }
}