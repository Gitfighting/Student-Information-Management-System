using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace StudentManageSystem.Model
{
    [Table("student_info")] // 指定生成的表名
    public class Student
    {
        [Key] // 主键
        [Column("stu_id")] // 数据库字段名
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int stuId { get; set; }

        [Column("stu_name")]
        [Required] // 非空约束
        [MaxLength(50)] // 最大长度
        public string stuName { get; set; }

        [Column("stu_gender")]
        [MaxLength(2)] // 如“男”“女”
        public string stuGender { get; set; }

        [Column("stu_age")]
        [Range(1, 150)] // 年龄范围约束
        public int stuAge { get; set; }

        [Column("stu_major")]
        [MaxLength(100)]
        public string stuMajor { get; set; }

        [Column("stu_birth")]
        public DateTime stuBirth { get; set; }

        [Column("stu_email")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)] // 邮箱格式验证
        public string stuEmail { get; set; }

        [Column("stu_phone")]
        [MaxLength(20)]
        public string stuPhone { get; set; }

        [Column("stu_pwd")]
        [Required]
        [MaxLength(50)]
        public string stuPwd { get; set; }
    }
}
