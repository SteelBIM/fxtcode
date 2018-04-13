using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class UserFeedBackDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        /// 查询所有TB_UserFeedback
        /// </summary>
        /// <returns></returns>
        public IList<TB_UserFeedback> GetUserFeedbackList()
        {
            IList<TB_UserFeedback> list = SelectAll<TB_UserFeedback>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserFeedback> GetUserFeedbackList(string where)
        {
            IList<TB_UserFeedback> list = Search<TB_UserFeedback>(where);
            return list;
        }

        /// <summary>
        /// 通过ID获取反馈信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserFeedback> GetFeedBackByID(string where)
        {
            return Search<TB_UserFeedback>(where, "");
        }

        /// <summary>
        /// 修改反馈信息
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool UpdateFeedBackInfo(TB_UserFeedback feedBackInfo)
        {
            bool b = Update<TB_UserFeedback>(feedBackInfo);
            return b;
        }

        /// <summary>
        /// 添加反馈信息
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool InsertFeedBackInfo(TB_UserFeedback feedBackInfo)
        {
            return manage.Insert<TB_UserFeedback>(feedBackInfo);
        }
    }
}
