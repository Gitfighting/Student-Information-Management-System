using System.Collections.Generic;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;
using StudentManageSystem.Common;
using System;
using System.Linq;

namespace StudentManageSystem.BLL
{
    public class LoginLogBll : ILoginLogBll
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginLogBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 查询所有登录日志 - 纯查询逻辑
        /// </summary>
        public ResultVO QueryLoginLogs()
        {
            var logRepo = _unitOfWork.GetRepository<LoginLog>();
            var logs = logRepo.GetAll();
            return new ResultVO { code = 2, message = $"查询到{logs.Count}条日志", data = logs };
        }

        /// <summary>
        /// 按条件查询登录日志 - 专注于查询业务逻辑
        /// </summary>
        public ResultVO QueryLoginLogsByConditions(string userType, string operationType)
        {
            var logRepo = _unitOfWork.GetRepository<LoginLog>();
            var query = logRepo.GetAll().AsQueryable();

            // 业务逻辑：按用户类型筛选
            if (!string.IsNullOrEmpty(userType))
                query = query.Where(log => log.userType == userType);

            // 业务逻辑：按操作类型筛选
            if (!string.IsNullOrEmpty(operationType))
                query = query.Where(log => log.operationType == operationType);

            var filteredLogs = query.ToList();
            return new ResultVO { code = 2, message = $"查询到{filteredLogs.Count}条日志", data = filteredLogs };
        }

        /// <summary>
        /// 添加登录日志 - 专注于核心业务逻辑
        /// </summary>
        public ResultVO AddLoginLog(LoginLog log)
        {
            // 业务逻辑：设置日志记录时间（如果未设置）
            if (log.loginTime == default(DateTime))
            {
                log.loginTime = DateTime.Now;
            }

            // 业务逻辑：标准化用户ID（处理空值情况）
            if (string.IsNullOrEmpty(log.userId))
            {
                log.userId = "-"; // 统一的空值标识
            }

            var logRepo = _unitOfWork.GetRepository<LoginLog>();
            logRepo.Add(log);

            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "登录日志记录成功", data = null }
                : new ResultVO { code = 1, message = "登录日志记录失败", data = null };
        }
    }
}