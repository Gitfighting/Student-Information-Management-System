using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(DbContext context) : base(context) { }

        public IList<Student> SearchByConditions(QueryStudentModel model)
        {
            var query = _dbSet.AsQueryable();

            if (model.StuId.HasValue)
                query = query.Where(s => s.stuId == model.StuId.Value);

            if (!string.IsNullOrWhiteSpace(model.StuName))
                query = query.Where(s => s.stuName.Contains(model.StuName));

            if (!string.IsNullOrWhiteSpace(model.StuGender))
                query = query.Where(s => s.stuGender == model.StuGender);

            if (model.StuAge.HasValue)
                query = query.Where(s => s.stuAge == model.StuAge.Value);

            if (!string.IsNullOrWhiteSpace(model.StuMajor))
                query = query.Where(s => s.stuMajor == model.StuMajor);

            return query.OrderBy(s => s.stuId).ToList();
        }
    }
}

