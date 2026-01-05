using System;

namespace StudentManageSystem.Model
{
    /// <summary>
    /// 课程查询条件模型
    /// </summary>
    public class QueryCourseModel
    {
        public int? courseId { get; set; }          // 课程编号（精确查询）
        public string courseName { get; set; }      // 课程名称（模糊查询）
        public int? preCourseId { get; set; }       // 先修课程ID（精确查询）
        public decimal? courseCredit { get; set; }  // 学分（精确查询）
    }
}