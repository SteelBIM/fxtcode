using KingsSun.SpokenBroadcas.DAL;
using Kingsun.SpokenBroadcas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.BLL
{
    public class UserLearnBLL
    {
        UserLearnDAL dal = new UserLearnDAL();
        /// <summary>
        /// 根据条件查询Tb_UserLearn
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_UserLearn> GetUserLearn(string strWhere, string orderby = "")
        {
            return dal.GetUserLearn(strWhere, orderby);
        }
        /// <summary>
        /// 添加用户学习信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserLearn(Tb_UserLearn model)
        {
            return dal.AddUserLearn(model);
        }
        /// <summary>
        /// 根据CoursePeriodTimeID获取课时信息
        /// </summary>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodByCoursePeriodTimeID(int CoursePeriodTimeID)
        {
            return dal.GetCoursePeriodByCoursePeriodTimeID(CoursePeriodTimeID);
        }
    }
}
