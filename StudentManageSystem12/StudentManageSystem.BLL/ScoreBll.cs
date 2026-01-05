using System;
using System.Collections.Generic;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public class ScoreBll : IScoreBll
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IScoreRepository _scoreRepo;

        public ScoreBll(IUnitOfWork unitOfWork, IScoreRepository scoreRepo)
        {
            _unitOfWork = unitOfWork;
            _scoreRepo = scoreRepo;
        }

        /// <summary>
        /// 添加成绩 - 专注于业务唯一性验证和业务逻辑
        /// </summary>
        public ResultVO AddScore(Score score, bool isAdmin)
        {
            var studentRepo = _unitOfWork.GetRepository<Student>();
            var courseRepo = _unitOfWork.GetRepository<Course>();

            // 业务逻辑：验证学生存在性
            if (!studentRepo.Exists(s => s.stuId == score.stuId))
                return new ResultVO { code = 0, message = $"学号{score.stuId}不存在", data = null };

            // 业务逻辑：验证课程存在性
            if (!courseRepo.Exists(c => c.courseId == score.courseId))
                return new ResultVO { code = 0, message = $"课程号{score.courseId}不存在", data = null };

            // 业务逻辑：验证成绩记录唯一性（一个学生一门课程只能有一条成绩记录）
            if (_scoreRepo.Exists(s => s.stuId == score.stuId && s.courseId == score.courseId))
                return new ResultVO { code = 0, message = "该学生已存在此课程成绩，无法重复添加", data = null };

            // 业务逻辑：计算是否及格
            score.isPass = score.score >= 60;

            _scoreRepo.Add(score);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "成绩添加成功", data = score }
                : new ResultVO { code = 1, message = "成绩添加失败", data = null };
        }

        /// <summary>
        /// 删除成绩 - 专注于存在性验证
        /// </summary>
        public ResultVO DeleteScore(int scoreId, bool isAdmin)
        {
            // 业务逻辑：验证成绩记录存在性
            if (_scoreRepo.GetById(scoreId) == null)
                return new ResultVO { code = 0, message = "成绩记录不存在", data = null };

            _scoreRepo.DeleteById(scoreId);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = $"成绩记录{scoreId}删除成功", data = null }
                : new ResultVO { code = 1, message = "成绩删除失败", data = null };
        }

        /// <summary>
        /// 更新成绩 - 专注于存在性、关联性和唯一性验证
        /// </summary>
        public ResultVO UpdateScore(Score score, bool isAdmin)
        {
            // 业务逻辑：验证成绩记录存在性
            if (_scoreRepo.GetById(score.scoreId) == null)
                return new ResultVO { code = 0, message = "成绩记录不存在", data = null };

            var studentRepo = _unitOfWork.GetRepository<Student>();
            var courseRepo = _unitOfWork.GetRepository<Course>();

            // 业务逻辑：验证学生存在性
            if (!studentRepo.Exists(s => s.stuId == score.stuId))
                return new ResultVO { code = 0, message = $"学号{score.stuId}不存在", data = null };

            // 业务逻辑：验证课程存在性
            if (!courseRepo.Exists(c => c.courseId == score.courseId))
                return new ResultVO { code = 0, message = $"课程号{score.courseId}不存在", data = null };

            // 业务逻辑：验证成绩记录唯一性（除了当前记录外，不能有其他相同学生课程的记录）
            if (_scoreRepo.Exists(s => s.stuId == score.stuId && s.courseId == score.courseId && s.scoreId != score.scoreId))
                return new ResultVO { code = 0, message = "该学生已存在此课程的其他成绩记录", data = null };

            // 业务逻辑：重新计算是否及格
            score.isPass = score.score >= 60;

            _scoreRepo.Update(score);
            return _unitOfWork.SaveChanges() > 0
                ? new ResultVO { code = 2, message = "成绩更新成功", data = score }
                : new ResultVO { code = 1, message = "成绩更新失败", data = null };
        }

        /// <summary>
        /// 多条件查询成绩 - 纯查询逻辑
        /// </summary>
        public ResultVO QueryScores(QueryScoreModel queryModel)
        {
            var scores = _scoreRepo.SearchByConditions(queryModel);
            return new ResultVO { code = 2, message = $"查询到{scores.Count}条成绩记录", data = scores };
        }

        /// <summary>
        /// 根据ID查询成绩 - 纯查询逻辑
        /// </summary>
        public ResultVO GetScoreById(int scoreId)
        {
            var score = _scoreRepo.GetById(scoreId);
            return score != null
                ? new ResultVO { code = 2, message = "查询成功", data = score }
                : new ResultVO { code = 0, message = "成绩记录不存在", data = null };
        }

        /// <summary>
        /// 查询所有成绩 - 纯查询逻辑
        /// </summary>
        public ResultVO GetAllScores()
        {
            var scores = _scoreRepo.GetAll();
            return new ResultVO { code = 2, message = $"共{scores.Count}条成绩记录", data = scores };
        }
    }
}