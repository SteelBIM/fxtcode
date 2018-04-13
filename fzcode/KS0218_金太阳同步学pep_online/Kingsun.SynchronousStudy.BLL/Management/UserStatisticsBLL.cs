using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.BLL.Management
{
    public class UserStatisticsBLL
    {
        public List<UserStatisticsModel> GetUserStatistic(string where) 
        {
            UserStatisticsDal dal = new UserStatisticsDal();
            return dal.GetUserStatistics(where);
        }
    }
}
