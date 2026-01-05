using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public interface ICourseBll
    {
        ResultVO AddCourse(Course course, bool isAdmin);
        ResultVO DeleteCourse(int courseId, bool isAdmin);
        ResultVO UpdateCourse(Course course, bool isAdmin);
        ResultVO QueryCourses(QueryCourseModel queryModel);
        ResultVO GetCourseById(int courseId);
        ResultVO GetAllCourses();
    }
}
