using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class ModuleConfigurationDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        /// 查询所有TB_ModuleConfiguration
        /// </summary>
        /// <returns></returns>
        public IList<TB_ModuleConfiguration> GetModuleList()
        {
            IList<TB_ModuleConfiguration> list = SelectAll<TB_ModuleConfiguration>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleConfiguration> GetModuleList(string where)
        {
            IList<TB_ModuleConfiguration> list = Search<TB_ModuleConfiguration>(where);
            return list;
        }

        /// <summary>
        /// 通过ID获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleConfiguration> GetModuleByID(string where)
        {
            return Search<TB_ModuleConfiguration>(where, "");
        }

        /// <summary>
        /// 通过ID FirstTitileID SecondTitleID 获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_ModuleConfiguration GetModuleByFirstSecondID(string where)
        {
            return SelectByCondition<TB_ModuleConfiguration>(where);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetModuleByWhere(string where)
        {
            DataSet ds = ExecuteSql(where);
          //  DataTable dt = ds.Tables[0];

            return ds;
        }
        
        /// <summary>
        /// 修改模块信息
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool UpdateModuleInfo(TB_ModuleConfiguration moduleInfo)
        {
            bool b = Update<TB_ModuleConfiguration>(moduleInfo);
            return b;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="feedBackInfo"></param>
        /// <returns></returns>
        public bool InsertModuleInfo(TB_ModuleConfiguration moduleInfo)
        {
            return manage.Insert<TB_ModuleConfiguration>(moduleInfo);
        }
    }
}
