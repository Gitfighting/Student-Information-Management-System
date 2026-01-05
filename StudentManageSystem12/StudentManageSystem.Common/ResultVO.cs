
namespace StudentManageSystem.Common
{
    public class ResultVO
    {
        public int code { get; set; }          // 状态码：0=失败（用户存在/不存在），1=业务失败，2=成功
        public string message { get; set; }    // 提示信息
        public object data { get; set; }       // 返回数据（如学生信息、成绩列表）


    }
}
