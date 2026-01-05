    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManageSystem.Model
{
    [Table("login_log")]
    public class LoginLog
    {
        [Key]
        [Column("log_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 自增主键
        public int logId { get; set; }

        [Column("login_time")]
        [Required]
        public DateTime loginTime { get; set; }

        [Column("operation_type")]
        [MaxLength(50)]
        public string operationType { get; set; }

        [Column("user_id")]
        [MaxLength(50)]
        public string userId { get; set; }

        [Column("user_type")]
        [MaxLength(20)] // 如“student”“admin”
        public string userType { get; set; }
    }
}
