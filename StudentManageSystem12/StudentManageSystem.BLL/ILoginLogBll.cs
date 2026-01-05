using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public interface ILoginLogBll
    {
        ResultVO AddLoginLog(LoginLog log);
        ResultVO QueryLoginLogs();
        ResultVO QueryLoginLogsByConditions(string userType, string operationType);
    }
}
