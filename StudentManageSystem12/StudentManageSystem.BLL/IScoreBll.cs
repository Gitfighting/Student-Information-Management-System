using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManageSystem.Common;
using StudentManageSystem.Model;

namespace StudentManageSystem.BLL
{
    public interface IScoreBll
    {
        ResultVO AddScore(Score score, bool isAdmin);
        ResultVO UpdateScore(Score score, bool isAdmin);
        ResultVO DeleteScore(int scoreId, bool isAdmin);
        ResultVO QueryScores(QueryScoreModel queryModel);
        ResultVO GetScoreById(int scoreId);
        ResultVO GetAllScores();
    }
}
