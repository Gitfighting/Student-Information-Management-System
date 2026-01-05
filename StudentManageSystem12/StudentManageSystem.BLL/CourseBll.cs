using System;
using System.Collections.Generic;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public class CourseBll : ICourseBll
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseRepository _courseRepo;

        public CourseBll(IUnitOfWork unitOfWork, ICourseRepository courseRepo)
        {
            _unitOfWork = unitOfWork;
            _courseRepo = courseRepo;
        }

        /// <summary>
        /// 添加课程 - 专注于业务唯一性和关联性验证
        /// </summary>
        public ResultVO AddCourse(Course course, bool isAdmin)
        {
            // 业务逻辑：课程编号唯一性验证
            if (_courseRepo.GetById(course.courseId) != null)
                return new ResultVO { code = 0, message = $"课程编号{course.courseId}已存在", data = null };

            // 业务逻辑：先修课程存在性验证
            if (course.preCourseId.HasValue && _courseRepo.GetById(course.preCourseId.Value) == null)
                return new ResultVO { code = 0, message = $"先修课程ID{course.preCourseId}不存在", data = null };

            _courseRepo.Add(course);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "课程添加成功", data = course }
                : new ResultVO { code = 1, message = "课程添加失败", data = null };
        }

        /// <summary>
        /// 删除课程 - 专注于存在性验证
        /// </summary>
        public ResultVO DeleteCourse(int courseId, bool isAdmin)
        {
            // 业务逻辑：课程存在性验证
            if (_courseRepo.GetById(courseId) == null)
                return new ResultVO { code = 0, message = "课程不存在", data = null };

            _courseRepo.DeleteById(courseId);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = $"课程{courseId}删除成功", data = null }
                : new ResultVO { code = 1, message = "课程删除失败", data = null };
        }

        /// <summary>
        /// 更新课程 - 专注于存在性和关联性验证
        /// </summary>
        public ResultVO UpdateCourse(Course course, bool isAdmin)
        {
            // 业务逻辑：课程存在性验证
            if (_courseRepo.GetById(course.courseId) == null)
                return new ResultVO { code = 0, message = "课程不存在", data = null };

            // 业务逻辑：先修课程存在性验证
            if (course.preCourseId.HasValue && _courseRepo.GetById(course.preCourseId.Value) == null)
                return new ResultVO { code = 0, message = "先修课程不存在", data = null };

            _courseRepo.Update(course);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "课程更新成功", data = course }
                : new ResultVO { code = 1, message = "课程更新失败", data = null };
        }

        /// <summary>
        /// 查询课程 - 纯查询逻辑
        /// </summary>
        public ResultVO QueryCourses(QueryCourseModel queryModel)
        {
            var courses = _courseRepo.SearchByConditions(queryModel);
            return new ResultVO { code = 2, message = $"查询到{courses.Count}门课程", data = courses };
        }

        public ResultVO GetCourseById(int courseId)
        {
            var course = _courseRepo.GetById(courseId);
            return course != null
                ? new ResultVO { code = 2, message = "查询成功", data = course }
                : new ResultVO { code = 0, message = "课程不存在", data = null };
        }

        public ResultVO GetAllCourses()
        {
            var courses = _courseRepo.GetAll();
            return new ResultVO { code = 2, message = $"共{courses.Count}门课程", data = courses };
        }
    }
}