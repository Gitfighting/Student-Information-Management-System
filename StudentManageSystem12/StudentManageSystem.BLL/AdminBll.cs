using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public class AdminBll : IAdminBll
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 管理员登录验证 - 专注于业务逻辑
        /// </summary>
        public ResultVO CheckAdminLogin(string adminName, string adminPwd)
        {
            var adminRepo = _unitOfWork.GetRepository<Admin>();
            var admin = adminRepo.FirstOrDefault(a => a.AdminName == adminName);

            // 业务逻辑：用户不存在
            if (admin == null)
                return new ResultVO { code = 0, message = "管理员账号不存在", data = null };

            // 业务逻辑：密码验证
            if (admin.AdminPwd != adminPwd)
                return new ResultVO { code = 1, message = "管理员密码错误", data = null };

            // 业务逻辑：返回新对象，隐藏敏感信息（避免修改原实体）
            var adminInfo = new Admin
            {
                AdminId = admin.AdminId,
                AdminName = admin.AdminName,
                AdminPwd = "******" // 只在返回的副本中隐藏密码
            };
            return new ResultVO { code = 2, message = "管理员登录成功", data = adminInfo };
        }
    }
}
