using System;

namespace StudentManageSystem.Model
{
    public class QueryStudentModel
    {
        public int? StuId { get; set; }          // 学号精确查询
        public string StuName { get; set; }      // 姓名模糊查询
        public string StuGender { get; set; }    // 性别精确查询
        public int? StuAge { get; set; }         // 年龄精确查询
        public string StuMajor { get; set; }     // 专业精确查询
    }
}