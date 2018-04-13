using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using System.Data;

namespace Kingsun.SynchronousStudy.BLL
{
    /// <summary>
    /// 学习报告BLL
    /// </summary>
    public class LR_LearningReportBLL
    {
        LR_LearningReportDAL lrManagement = new LR_LearningReportDAL();

        /// <summary>
        /// 获取班级列表:日期  条件班级ids
        /// </summary>
        /// <param name="lsStuid">查询班级下的学生</param>
        /// <param name="time">查询时间</param>
        /// <param name="editionId">教师版本</param>
        /// <returns></returns>
        public DataTable QueryClassList(List<int> lsStuid, string time,int editionId)
        {
            DataTable dt = lrManagement.QueryClassList(lsStuid, time, editionId);

            return dt;
        }

        /// <summary>
        /// 根据班级[学生列表ID]，时间查询日期内学生趣配音的完成数量
        /// </summary>
        /// <param name="lsStuid">查询班级下的学生</param>
        /// <param name="time">查询时间</param>
        /// <param name="UnitId">UnitId</param>
        /// <returns></returns>
        public DataTable GetVideoDetailsCountByClass(List<int> lsStuid, string time, string UnitId)
        {
            DataTable dt = lrManagement.GetVideoDetailsCountByClass(lsStuid, time, UnitId);
            return dt;
        }

        /// <summary>
        /// 获取教师版本 --根据教师ID
        /// </summary>
        /// <param name="UserID">教师ID</param>
        /// <returns></returns>
        public IList<TB_UserEditionInfo> GetUserEditionInfo(string UserID)
        {
            IList<TB_UserEditionInfo> lsUser = lrManagement.GetUserEditionInfo(UserID);
            return lsUser;
        }

        /// <summary>
        /// 获取所有绑定微信用户列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_UserOpenID> GetUserOpenID()
        {
            IList<TB_UserOpenID> lsUserOpen = lrManagement.GetUserOpenID();
            return lsUserOpen;
        }
    }
}
