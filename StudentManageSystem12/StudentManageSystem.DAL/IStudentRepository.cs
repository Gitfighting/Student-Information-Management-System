using System.Collections.Generic;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public interface IStudentRepository : IRepository<Student>
    {
        IList<Student> SearchByConditions(QueryStudentModel queryModel);
    }
}

