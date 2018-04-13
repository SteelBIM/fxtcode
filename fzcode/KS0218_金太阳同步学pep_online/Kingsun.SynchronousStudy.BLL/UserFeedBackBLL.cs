using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserFeedBackBLL
    {
        UserFeedBackDAL userFeedBackDAL = new UserFeedBackDAL();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<TB_UserFeedback> GetFeedBackList()
        {
            return userFeedBackDAL.GetUserFeedbackList();
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_UserFeedback> GetFeedBackList(string where)
        {
            return userFeedBackDAL.GetUserFeedbackList(where);
        }

        /// <summary>
        /// 通过ID获取反馈信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserFeedback> GetFeedBackByID(string where)
        {
            return userFeedBackDAL.GetFeedBackByID(where);
        }

        /// <summary>
        /// 修改App信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        public bool UpdateFeedBackInfo(TB_UserFeedback feedBackInfo)
        {
            bool b = userFeedBackDAL.UpdateFeedBackInfo(feedBackInfo);
            return b;
        }

        /// <summary>
        /// 添加App
        /// </summary>
        /// <param name="appInfo">userInfo</param>
        /// <returns></returns>
        public bool InsertFeedBackInfo(TB_UserFeedback feedBackInfo)
        {
            return userFeedBackDAL.InsertFeedBackInfo(feedBackInfo);
        }
    }
}
