using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class APPManagementDAL : BaseManagement
    {
        public TB_APPManagement GetAPPManagement(string id)
        {
            TB_APPManagement course = SelectByCondition<TB_APPManagement>("ID='" + id + "'");
            return course;
        }

        public TB_APPManagement GetAPPByEditionID(string id)
        {
            TB_APPManagement course = SelectByCondition<TB_APPManagement>("VersionID=" + id);
            return course;
        }

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_APPManagement> QueryAPPList()
        {
            IList<TB_APPManagement> list = Search<TB_APPManagement>("1=1 ORDER BY CreateDate DESC");
            return list;
        }


        /// <summary>
        /// 通过用户编号获取用户版本
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public TB_UserEditionInfo GetUserEditionByID(string userid)
        {
            return SelectByCondition<TB_UserEditionInfo>("UserID=" + userid);
        }

        /// <summary>
        /// 新增用户版本信息
        /// </summary>
        /// <param name="editioninfo"></param>
        /// <returns></returns>
        public bool AddUserEditionInfo(TB_UserEditionInfo editioninfo)
        {
            return Insert<TB_UserEditionInfo>(editioninfo);
        }

        /// <summary>
        /// 更新用户版本信息
        /// </summary>
        /// <param name="editioninfo"></param>
        /// <returns></returns>
        public bool UpdateUserEditionInfo(TB_UserEditionInfo editioninfo)
        {
            return Update<TB_UserEditionInfo>(editioninfo);
        }
    }
}
