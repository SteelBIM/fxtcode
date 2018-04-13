using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.BLL
{
    public class CouponBll
    {
        CouponDal nlr = new CouponDal();
        /// <summary>
        /// 根据条件获取优惠卷列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<CouponListModel> GetTicketListByStrWhere(string strWhere)
        {
            return nlr.GetTicketListByStrWhere(strWhere);
        }
        /// <summary>
        /// 根据版本ID获取TB_CurriculumManage
        /// </summary>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        public List<TB_CurriculumManage> GetCurriculumManageList(string EditionID)
        {
            return nlr.GetCurriculumManageList(EditionID);
        }
        /// <summary>
        /// 根据版本ID删除优惠卷
        /// </summary>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        public bool DelTicketInfo(string EditionID)
        {
            return nlr.DelTicketInfo(EditionID);
        }
        /// <summary>
        /// 根据版本ID，开始时间和结束时间判断卷的使用时间是否重合(true:没有，false:有)
        /// </summary>
        /// <param name="EditionID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns>true:没有，false:有</returns>
        public bool CheckExistsTime(string EditionID, string StartDate, string EndDate, string strWhere)
        {
            return nlr.CheckExistsTime(EditionID, StartDate, EndDate, strWhere);
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
            return nlr.UpdateTicket(EditionID, StartDate, EndDate, Status);
        }
    }
}
