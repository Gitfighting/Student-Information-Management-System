using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(DbContext context) : base(context) { }

        public IList<Course> SearchByConditions(QueryCourseModel model)
        {
            var query = _dbSet.AsNoTracking()
                             .Include(c => c.PreCourse)
                             .AsQueryable();

            if (model.courseId.HasValue)
                query = query.Where(c => c.courseId == model.courseId.Value);

            if (!string.IsNullOrWhiteSpace(model.courseName))
                query = query.Where(c => c.courseName.Contains(model.courseName));

            var courses = query.ToList();

            // 填充先修课程名称并清除导航属性（避免循环引用）
            foreach (var course in courses)
            {
                course.preCourseName = course.PreCourse?.courseName;
                course.PreCourse = null; // 清除导航属性，避免JSON序列化问题
            }

            return courses;
        }

        /// <summary>
        /// 获取所有课程（包含先修课程信息）
        /// </summary>
        public new IList<Course> GetAll()
        {
            var courses = _dbSet.AsNoTracking()
                               .Include(c => c.PreCourse)
                               .ToList();

            // 填充先修课程名称并清除导航属性（避免循环引用）
            foreach (var course in courses)
            {
                course.preCourseName = course.PreCourse?.courseName;
                course.PreCourse = null; // 清除导航属性，避免JSON序列化问题
            }

            return courses;
        }
    }
}

