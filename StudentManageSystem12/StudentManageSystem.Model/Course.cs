using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManageSystem.Model
{
    [Table("course_info")]
    public class Course
    {
        [Key]
        [Column("course_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 课程编号手动输入
        public int courseId { get; set; }

        [Column("course_name")]
        [Required]
        [MaxLength(100)]
        public string courseName { get; set; }

        [Column("course_credit")]
        [Range(0, 10)] // 学分范围示例
        public decimal courseCredit { get; set; }

        [Column("pre_course_id")] // 先修课程ID（可为空）
        public int? preCourseId { get; set; }

        // 导航属性（关联自身，先修课程）
        [ForeignKey("preCourseId")]
        public Course PreCourse { get; set; }

        // 前端展示字段（不映射到数据库）
        [NotMapped]
        public string preCourseName { get; set; }
    }
}
