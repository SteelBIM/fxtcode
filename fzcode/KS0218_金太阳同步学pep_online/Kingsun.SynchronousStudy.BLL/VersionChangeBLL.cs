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
    public class VersionChangeBLL
    {
        VersionChangeDAL versionChangeDAL = new VersionChangeDAL();
        /// <summary>
        /// 修改模块信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool UpdateModuleInfo(TB_VersionChange moduleInfo)
        {
            bool b = versionChangeDAL.UpdateModuleInfo(moduleInfo);
            return b;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool InsertModuleInfo(TB_VersionChange moduleInfo)
        {
            return versionChangeDAL.InsertModuleInfo(moduleInfo);
        }

        /// <summary>
        /// 获取某个模块的最新资源
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_VersionChange GetNewVersionChange(string where)
        {
            return versionChangeDAL.GetNewVersionChange(where);
        }

        /// <summary>
        /// 获取某个模块的最新资源(优学专用)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_VersionChange_YX GetNewVersionChange_YX(string where)
        {
            return versionChangeDAL.GetNewVersionChange_YX(where);
        }

        /// <summary>
        /// 通过ID获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange> GetModuleByID(string where)
        {
            return versionChangeDAL.GetModuleByID(where);
        }

        /// <summary>
        /// 通过查询条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange> GetModuleByWhere(string where)
        {
           return versionChangeDAL.GetModuleList(where);
        }
        /// <summary>
        /// 通过查询条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange_YX> GetModuleByWhere_YX(string where)
        {
            var re = versionChangeDAL.GetModuleList_YX(where);
            return re;
        }
    }
}
