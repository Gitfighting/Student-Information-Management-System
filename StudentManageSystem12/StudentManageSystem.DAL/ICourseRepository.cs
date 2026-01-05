using System.Collections.Generic;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public interface ICourseRepository : IRepository<Course>
    {
        IList<Course> SearchByConditions(QueryCourseModel queryModel);
    }
}

