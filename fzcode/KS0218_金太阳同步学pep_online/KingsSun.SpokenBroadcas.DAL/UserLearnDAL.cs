using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KingsSun.SpokenBroadcas.DAL
{
    public class UserLearnDAL : BaseManagement
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 根据条件查询Tb_UserLearn
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_UserLearn> GetUserLearn(string strWhere, string orderby = "")
        {
            try
            {
                return Search<Tb_UserLearn>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }

        /// <summary>
        /// 添加用户学习信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserLearn(Tb_UserLearn model)
        {
            try
            {
                string sql = string.Format(@"insert into Tb_UserLearn(UserID,CoursePeriodTimeID,OutTimes,StartTime,EndTime,UserName,TrueName,TelePhone,CourseID,
                CourseName,CoursePeriodID,CoursePeriodName,NewPrice,CourseStartTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                    model.UserID, model.CoursePeriodTimeID, model.OutTimes, model.StartTime, model.EndTime, model.UserName, model.TrueName, model.TelePhone, model.CourseID, model.CourseName
                    , model.CoursePeriodID, model.CoursePeriodName, model.NewPrice, model.CourseStartTime);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql);
                if (count > 0)
                {
                    return true;
                }
                return false;
                //return Insert<Tb_UserLearn>(model);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return false;
        }

        /// <summary>
        /// 根据CoursePeriodTimeID获取课时信息
        /// </summary>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodByCoursePeriodTimeID(int CoursePeriodTimeID)
        {
            try
            {
                string sql = string.Format(@"select  c.ID'CourseID',c.Name'CourseName',b.ID'CoursePeriodID',b.Name'CoursePeriodName',b.NewPrice,
   CONVERT(varchar(100),a.StartTime, 112)+' '+CONVERT(varchar(100),a.StartTime, 24)+'-'+CONVERT(varchar(100),a.EndTime, 24)'CourseStartTime' 
   from Tb_CoursePeriodTime a left join 
   Tb_CoursePeriod b on a.CoursePeriodID=b.ID
   left join Tb_Course c on b.CourseID=c.ID
   where a.ID='{0}'", CoursePeriodTimeID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
    }
}
