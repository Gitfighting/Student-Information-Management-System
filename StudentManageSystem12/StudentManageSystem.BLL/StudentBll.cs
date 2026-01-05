using System.Collections.Generic;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public class StudentBll : IStudentBll
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepo;

        public StudentBll(IUnitOfWork unitOfWork, IStudentRepository studentRepo)
        {
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
        }

        /// <summary>
        /// 学生注册 - 专注于业务唯一性验证
        /// </summary>
        public ResultVO Register(Student student)
        {
            // 业务逻辑：用户名唯一性验证
            if (_studentRepo.Exists(s => s.stuName == student.stuName))
                return new ResultVO { code = 0, message = "用户名已被注册", data = null };

            _studentRepo.Add(student);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "注册成功", data = null }
                : new ResultVO { code = 1, message = "注册失败", data = null };
        }

        /// <summary>
        /// 学生登录验证 - 专注于认证逻辑
        /// </summary>
        public ResultVO CheckLogin(string stuName, string stuPwd)
        {
            var student = _studentRepo.FirstOrDefault(s => s.stuName == stuName);

            // 业务逻辑：用户存在性验证
            if (student == null)
                return new ResultVO { code = 0, message = "用户不存在", data = null };

            // 业务逻辑：密码验证
            if (student.stuPwd != stuPwd)
                return new ResultVO { code = 1, message = "密码错误", data = null };

            // 业务逻辑：返回新对象，隐藏敏感信息（避免修改原实体）
            var studentInfo = new Student
            {
                stuId = student.stuId,
                stuName = student.stuName,
                stuGender = student.stuGender,
                stuAge = student.stuAge,
                stuMajor = student.stuMajor,
                stuBirth = student.stuBirth,
                stuEmail = student.stuEmail,
                stuPhone = student.stuPhone,
                stuPwd = "******" // 只在返回的副本中隐藏密码
            };
            return new ResultVO { code = 2, message = "登录成功", data = studentInfo };
        }

        /// <summary>
        /// 查询个人信息 - 专注于业务查询逻辑
        /// </summary>
        public ResultVO QueryPersonalInfo(int stuId)
        {
            var student = _studentRepo.GetById(stuId);
            if (student == null)
                return new ResultVO { code = 0, message = "学生信息不存在", data = null };

            // 业务逻辑：返回新对象，隐藏敏感信息（避免修改原实体）
            var studentInfo = new Student
            {
                stuId = student.stuId,
                stuName = student.stuName,
                stuGender = student.stuGender,
                stuAge = student.stuAge,
                stuMajor = student.stuMajor,
                stuBirth = student.stuBirth,
                stuEmail = student.stuEmail,
                stuPhone = student.stuPhone,
                stuPwd = "******" // 只在返回的副本中隐藏密码
            };
            return new ResultVO { code = 2, message = "查询成功", data = studentInfo };
        }

        /// <summary>
        /// 更新学生信息 - 专注于存在性验证
        /// </summary>
        public ResultVO UpdatePersonalInfo(Student student)
        {
            // 业务逻辑：验证学生是否存在
            if (_studentRepo.GetById(student.stuId) == null)
                return new ResultVO { code = 0, message = "学生不存在", data = null };

            _studentRepo.Update(student);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "修改成功", data = student.stuId }
                : new ResultVO { code = 1, message = "修改失败", data = null };
        }

        /// <summary>
        /// 删除学生 - 专注于存在性验证
        /// </summary>
        public ResultVO DeleteStudent(int stuId, bool isAdmin)
        {
            // 业务逻辑：验证学生是否存在
            if (_studentRepo.GetById(stuId) == null)
                return new ResultVO { code = 0, message = "学生不存在", data = null };

            _studentRepo.DeleteById(stuId);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = $"学号{stuId}删除成功", data = null }
                : new ResultVO { code = 1, message = "删除失败", data = null };
        }

        /// <summary>
        /// 添加学生 - 专注于业务唯一性验证
        /// </summary>
        public ResultVO AddStudent(Student student, bool isAdmin)
        {
            // 业务逻辑：学号唯一性验证
            if (_studentRepo.GetById(student.stuId) != null)
                return new ResultVO { code = 0, message = $"学号{student.stuId}已存在", data = null };

            // 业务逻辑：用户名唯一性验证
            if (_studentRepo.Exists(s => s.stuName == student.stuName))
                return new ResultVO { code = 0, message = $"用户名{student.stuName}已存在", data = null };

            _studentRepo.Add(student);
            if (_unitOfWork.SaveChanges() > 0)
            {
                // 业务逻辑：返回新对象，隐藏敏感信息（避免修改原实体）
                var studentInfo = new Student
                {
                    stuId = student.stuId,
                    stuName = student.stuName,
                    stuGender = student.stuGender,
                    stuAge = student.stuAge,
                    stuMajor = student.stuMajor,
                    stuBirth = student.stuBirth,
                    stuEmail = student.stuEmail,
                    stuPhone = student.stuPhone,
                    stuPwd = "******" // 只在返回的副本中隐藏密码
                };
                return new ResultVO { code = 2, message = "添加成功", data = studentInfo };
            }
            return new ResultVO { code = 1, message = "添加失败", data = null };
        }

        /// <summary>
        /// 查询所有学生 - 纯查询逻辑
        /// </summary>
        public ResultVO QueryAllStudents(bool isAdmin)
        {
            var students = _studentRepo.GetAll();
            return new ResultVO { code = 2, message = $"共{students.Count}名学生", data = students };
        }

        /// <summary>
        /// 条件查询学生 - 纯查询逻辑
        /// </summary>
        public ResultVO QueryStudentsByConditions(QueryStudentModel queryModel, bool isAdmin)
        {
            var students = _studentRepo.SearchByConditions(queryModel);
            return new ResultVO { code = 2, message = $"共{students.Count}名学生", data = students };
        }

        public ResultVO QueryStudentsByMajor(string major, bool isAdmin)
        {
            return QueryStudentsByConditions(new QueryStudentModel { StuMajor = major }, isAdmin);
        }
    }
}
