using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using System.Data;

namespace Kingsun.SynchronousStudy.DAL
{
    /// <summary>
    /// 学习报告DAL
    /// </summary>
    public class LR_LearningReportDAL : BaseManagement
    {
        /// <summary>
        /// 获取班级列表:日期  条件班级ids
        /// </summary>
        /// <param name="lsStuid">查询班级下的学生</param>
        /// <param name="time">查询时间</param>
        /// <param name="editionId">教师版本</param>
        /// <returns></returns>
        public DataTable QueryClassList(List<int> lsStuid, string stime, int editionId)
        {
            DateTime time;
            if (stime != "")
            {
                time = Convert.ToDateTime(stime);
            }
            else
            {
                time = DateTime.Now;
            }
            string stuIds = string.Join(",", lsStuid.ToArray());

            //string sqlstr = "select ClassLongID,COUNT(ClassLongID) 'Count' from ( "
            //    + "select  UserID,ClassLongID  from View_LRClassListData  where CreateTime between '" + time.ToString("yyyy-MM-dd")
            //    + "' and  '" + time.AddDays(+1).ToString("yyyy-MM-dd") + "' and ClassLongID in ('" + classIds.Replace(",", "','") + "') "
            //    + " group by ClassLongID ,UserID ) a group by ClassLongID";

            string sqlstr = "select UserID,COUNT(UserID) 'Count' from [View_LRClassListData] where CreateTime between '" + time.ToString("yyyy-MM-dd 00:00:00")
                          + "' and  '" + time.ToString("yyyy-MM-dd 23:59:59") + "' and UserID in (" + stuIds + ") "
                          + " and BookID in (select BookID from TB_CurriculumManage where EditionID=" + editionId + " and State=1) group by UserID";

            DataSet ds = ExecuteSql(sqlstr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据班级[学生列表ID]，时间查询日期内学生趣配音的完成数量
        /// </summary>
        /// <param name="lsStuid">查询班级下的学生</param>
        /// <param name="time">查询时间</param>
        /// <param name="UnitId">UnitId</param>
        /// <returns></returns>
        public DataTable GetVideoDetailsCountByClass(List<int> lsStuid, string stime, string UnitId)
        {
            DateTime time;
            if (stime != "")
            {
                time = Convert.ToDateTime(stime);
            }
            else
            {
                return null;
            }
            string t = "";
            if (UnitId.IndexOf("_") >= 0)
            {
                t = " b.FirstTitleID='" + UnitId.Split('_')[0] + "' and b.SecondTitleID='" + UnitId.Split('_')[1] + "'	and";
            }
            else
            {
                t = " b.FirstTitleID='" + UnitId.Split('_')[0] + "' and";
            }

            string stuIds = string.Join(",", lsStuid.ToArray());
            //string sqlstr1 = "select Count(UserID) 'Count' from TB_UserVideoDetails where UserID in ("
            //                + stuIds + ") and CreateTime between '" + time.ToString("yyyy-MM-dd") + "' and '"
            //                + time.AddDays(+1).ToString("yyyy-MM-dd") + "'";

            string sqlstr = "select a.UserID from [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a left join [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b "
                            + " on a.BookID=b.BookID and a.VideoNumber=b.VideoNumber where " + t + " a.UserID in (" + stuIds
                            + ") and a.CreateTime between '" + time.ToString("yyyy-MM-dd 00:00:00") + "' and '"
                            + time.ToString("yyyy-MM-dd 23:59:59") + "' and a.State=1 group by UserID ";

            DataSet ds = ExecuteSql(sqlstr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取教师版本 --根据教师ID
        /// </summary>
        /// <param name="UserID">教师ID</param>
        /// <returns></returns>
        public IList<TB_UserEditionInfo> GetUserEditionInfo(string UserID)
        {
            IList<TB_UserEditionInfo> lsUser = Search<TB_UserEditionInfo>(" UserID='" + UserID + "'");
            return lsUser;
        }

        /// <summary>
        /// 获取所有绑定微信用户列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_UserOpenID> GetUserOpenID()
        {
            IList<TB_UserOpenID> lsUser = SelectAll<TB_UserOpenID>();
            return lsUser;
        }
    }
}
