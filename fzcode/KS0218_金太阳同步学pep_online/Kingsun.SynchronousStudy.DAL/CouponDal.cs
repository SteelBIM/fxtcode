using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.DAL
{
    /// <summary>
    /// 优惠卷
    /// </summary>
    public class CouponDal
    {
        /// <summary>
        /// 根据条件获取优惠卷列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<CouponListModel> GetTicketListByStrWhere(string strWhere)
        {
            string sql = string.Format(@"SELECT  EditionID,TextbookVersion,TicketName,Price,StartDate,EndDate,ImgUrl,Type,Status   from TB_Ticket a
                left join TB_CurriculumManage b
                on a.CourseID=b.BookID where  State=1 and Status=0 and EndDate>GETDATE()  {0}
                group by EditionID,TextbookVersion,TicketName,Price,StartDate,EndDate,ImgUrl,Type,Status ", strWhere);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<CouponListModel> list = JsonHelper.DataSetToIList<CouponListModel>(ds, 0);
            return list;
        }

        /// <summary>
        /// 根据版本ID获取TB_CurriculumManage
        /// </summary>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        public List<TB_CurriculumManage> GetCurriculumManageList(string EditionID)
        {
            string sql = string.Format(@"select * from TB_CurriculumManage  where EditionID={0} and State=1 ", EditionID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<TB_CurriculumManage> list = JsonHelper.DataSetToIList<TB_CurriculumManage>(ds, 0);
            return list;
        }
        /// <summary>
        /// 根据版本ID删除优惠卷
        /// </summary>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        public bool DelTicketInfo(string EditionID)
        {
            string sql = string.Format("  delete TB_Ticket where  CourseID in(  select BookID from TB_CurriculumManage  where EditionID={0} and State=1 )", EditionID);
            int count = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据版本ID，开始时间和结束时间判断卷的使用时间是否重合
        /// </summary>
        /// <param name="EditionID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns>true:没有，false:有</returns>
        public bool CheckExistsTime(string EditionID, string StartDate, string EndDate, string strWhere)
        {
            string sql = string.Format(@"SELECT  EditionID,TextbookVersion,TicketName,StartDate,EndDate  from TB_Ticket a
                left join TB_CurriculumManage b
                on a.CourseID=b.BookID where  State=1 and Status=0 and  EditionID={0} and (StartDate>'{1}' or EndDate<'{2}' {3})
                group by EditionID,TextbookVersion,TicketName,StartDate,EndDate", EditionID, EndDate, StartDate, strWhere);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 修改使用卷信息
        /// </summary>
        /// <param name="EditionID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateTicket(string EditionID, string StartDate, string EndDate, int Status)
        {
            string sql = string.Format(@"update TB_Ticket set Status={0},StartDate='{1}',EndDate='{2}' where CourseID in(select BookID from TB_CurriculumManage where EditionID={3}) and Status=0
                                        ", Status, StartDate, EndDate, EditionID);
            int count = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
