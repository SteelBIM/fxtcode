using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;


namespace Kingsun.SynchronousStudy.DAL
{
    public class ApplicationVersionDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        /// 获取App列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ApplicationVersion> QueryAppList(string where)
        {
            IList<TB_ApplicationVersion> appList = manage.Search<TB_ApplicationVersion>(where);
            return appList;
        }

        /// <summary>
        /// 通过ID获取App信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ApplicationVersion> GetAppByID(string where)
        {
            return Search<TB_ApplicationVersion>(where, "");
        }

        /// <summary>
        /// 修改App信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        public bool UpdateAppInfo(TB_ApplicationVersion appInfo)
        {
            bool b = Update<TB_ApplicationVersion>(appInfo);
            return b;
        }

        /// <summary>
        /// 添加App
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertApp(TB_ApplicationVersion appInfo)
        {
            return manage.Insert<TB_ApplicationVersion>(appInfo);
        }

    }
}
