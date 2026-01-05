using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public class ScoreRepository : RepositoryBase<Score>, IScoreRepository
    {
        public ScoreRepository(DbContext context) : base(context) { }

        public IList<Score> SearchByConditions(QueryScoreModel model)
        {
            var query = _dbSet.AsNoTracking()
                             .Include(s => s.Student)
                             .Include(s => s.Course)
                             .AsQueryable();

            if (model.stuId.HasValue)
                query = query.Where(s => s.stuId == model.stuId.Value);

            if (model.courseId.HasValue)
                query = query.Where(s => s.courseId == model.courseId.Value);

            var scores = query.ToList();

            // 填充展示字段并清除导航属性（避免循环引用）
            foreach (var score in scores)
            {
                score.stuName = score.Student?.stuName;
                score.courseName = score.Course?.courseName;
                score.Student = null; // 清除导航属性
                score.Course = null;  // 清除导航属性
            }

            return scores;
        }

        /// <summary>
        /// 获取所有成绩（包含学生和课程信息）
        /// </summary>
        public new IList<Score> GetAll()
        {
            var scores = _dbSet.AsNoTracking()
                              .Include(s => s.Student)
                              .Include(s => s.Course)
                              .ToList();

            // 填充展示字段并清除导航属性（避免循环引用）
            foreach (var score in scores)
            {
                score.stuName = score.Student?.stuName;
                score.courseName = score.Course?.courseName;
                score.Student = null; // 清除导航属性
                score.Course = null;  // 清除导航属性
            }

            return scores;
        }
    }
}

