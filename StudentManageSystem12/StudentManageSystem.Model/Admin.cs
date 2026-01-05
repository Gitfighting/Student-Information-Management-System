using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManageSystem.Model
{
    [Table("admin_info")]
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 自增主键
        public long AdminId { get; set; }

        [Column("admin_name")]
        [Required]
        [MaxLength(50)]
        [Index(IsUnique = true)] // 用户名唯一
        public string AdminName { get; set; }

        [Column("admin_pwd")]
        [Required]
        [MaxLength(50)]
        public string AdminPwd { get; set; }

        [Column("admin_nickname")]
        [MaxLength(50)]
        public string AdminNickname { get; set; }
    }
}
