using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManageSystem.Common;

namespace StudentManageSystem.BLL
{
    public interface IAdminBll
    {
        ResultVO CheckAdminLogin(string adminName, string adminPwd);
    }
}
