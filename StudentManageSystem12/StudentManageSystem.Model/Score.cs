using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManageSystem.Model
{
    [Table("score_info")]
    public class Score
    {
        [Key] // 主键（自增）
        [Column("score_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int scoreId { get; set; }

        [Column("stu_id")] // 外键（关联学生表）
        [Required]
        public int stuId { get; set; }

        [Column("course_id")] // 外键（关联课程表）
        [Required]
        public int courseId { get; set; }

        [Column("score")]
        [Range(0, 100)] // 成绩范围0-100
        public decimal score { get; set; }

        [Column("is_pass")]
        public bool isPass { get; set; }

        // 导航属性（关联学生表）
        [ForeignKey("stuId")]
        public Student Student { get; set; }

        // 导航属性（关联课程表）
        [ForeignKey("courseId")]
        public Course Course { get; set; }

        // 前端展示字段（不映射到数据库，用[NotMapped]标记）
        [NotMapped]
        public string stuName { get; set; }

        [NotMapped]
        public string courseName { get; set; }
    }
}
