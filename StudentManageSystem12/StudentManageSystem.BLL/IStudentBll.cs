using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public interface IStudentBll
    {
        ResultVO Register(Student student);
        ResultVO CheckLogin(string stuName, string stuPwd);
        ResultVO QueryPersonalInfo(int stuId);
        ResultVO UpdatePersonalInfo(Student student);
        ResultVO DeleteStudent(int stuId, bool isAdmin);
        ResultVO AddStudent(Student student, bool isAdmin);
        ResultVO QueryAllStudents(bool isAdmin);
        ResultVO QueryStudentsByConditions(QueryStudentModel queryModel, bool isAdmin);
        ResultVO QueryStudentsByMajor(string major, bool isAdmin);
    }
}
