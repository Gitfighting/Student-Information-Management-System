using System;

namespace StudentManageSystem.Model
{
    /// <summary>
    /// 成绩查询条件模型（精确查询）
    /// </summary>
    public class QueryScoreModel
    {
        public int? stuId { get; set; }      // 学号（精确查询）
        public int? courseId { get; set; }   // 课程号（精确查询）
        public decimal? score { get; set; }  // 成绩（精确查询）
    }
}