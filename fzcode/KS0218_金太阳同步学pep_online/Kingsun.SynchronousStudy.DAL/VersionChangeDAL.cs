using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class VersionChangeDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        /// 查询所有TB_VersionChange
        /// </summary>
        /// <returns></returns>
        public IList<TB_VersionChange> GetModuleList()
        {
            IList<TB_VersionChange> list = SelectAll<TB_VersionChange>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange> GetModuleList(string where)
        {
            IList<TB_VersionChange> list = Search<TB_VersionChange>(where);
            return list;
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange_YX> GetModuleList_YX(string where)
        {
            IList<TB_VersionChange_YX> list = Search<TB_VersionChange_YX>(where);
            return list;
        }

        /// <summary>
        /// 通过ID获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VersionChange> GetModuleByID(string where)
        {
            return Search<TB_VersionChange>(where, "");
        }

        /// <summary>
        /// 修改模块信息
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool UpdateModuleInfo(TB_VersionChange moduleInfo)
        {
            bool b = Update<TB_VersionChange>(moduleInfo);
            return b;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool InsertModuleInfo(TB_VersionChange moduleInfo)
        {
            return manage.Insert<TB_VersionChange>(moduleInfo);
        }

        /// <summary>
        /// 获取某个模块的最新资源
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_VersionChange GetNewVersionChange(string where) 
        {
            IList<TB_VersionChange> verchange = Search<TB_VersionChange>(where);
            if (verchange != null)
            {
                return verchange[0];
            }
            else 
            {
                return null;
            }
            
        }

        /// <summary>
        /// 获取某个模块的最新资源,优学专用
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_VersionChange_YX GetNewVersionChange_YX(string where)
        {
            IList<TB_VersionChange_YX> verchange = Search<TB_VersionChange_YX>(where);
            if (verchange != null)
            {
                return verchange[0];
            }
            else
            {
                return null;
            }

        }
    }
}
