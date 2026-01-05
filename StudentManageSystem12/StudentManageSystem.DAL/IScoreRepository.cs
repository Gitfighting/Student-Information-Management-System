using System.Collections.Generic;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public interface IScoreRepository : IRepository<Score>
    {
        IList<Score> SearchByConditions(QueryScoreModel queryModel);
    }
}

