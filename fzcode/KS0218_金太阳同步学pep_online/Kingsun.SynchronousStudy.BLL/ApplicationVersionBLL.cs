using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class ApplicationVersionBLL
    {
        ApplicationVersionDAL applicationVersionDAL = new ApplicationVersionDAL();

        /// <summary>
        /// 获取所有App信息列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_ApplicationVersion> QueryAppList(string where)
        {
            IList<TB_ApplicationVersion> list = applicationVersionDAL.QueryAppList(where);
            return list;
        }

        /// <summary>
        /// 通过ID获取App信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ApplicationVersion> GetAppByID(string where)
        {
            return applicationVersionDAL.GetAppByID(where);
        }

        /// <summary>
        /// 修改App信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        public bool UpdateAppInfo(TB_ApplicationVersion appInfo)
        {
            bool b = applicationVersionDAL.UpdateAppInfo(appInfo);
            return b;
        }

        /// <summary>
        /// 添加App
        /// </summary>
        /// <param name="appInfo">userInfo</param>
        /// <returns></returns>
        public bool InsertApp(TB_ApplicationVersion appInfo)
        {
            return applicationVersionDAL.InsertApp(appInfo);
        }

    }
}
