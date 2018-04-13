using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class ModuleConfigurationBLL
    {
        ModuleConfigurationDAL moduleConfigurationDAL = new ModuleConfigurationDAL();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<TB_ModuleConfiguration> GetModuleList()
        {
            return moduleConfigurationDAL.GetModuleList();
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_ModuleConfiguration> GetModuleList(string where)
        {
            return moduleConfigurationDAL.GetModuleList(where);
        }

        /// <summary>
        /// 通过ID获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleConfiguration> GetModuleByID(string where)
        {
            return moduleConfigurationDAL.GetModuleByID(where);
        }

        //通过条件获取信息
        public DataSet GetModuleConfigurationByWhere(string where) 
        {
           return moduleConfigurationDAL.GetModuleByWhere(where);
        }
        
        /// <summary>
        /// 通过ID FirstTitileID SecondTitleID 获取模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_ModuleConfiguration GetModuleByFirstSecondID(string where)
        {
            return moduleConfigurationDAL.GetModuleByFirstSecondID(where);
        }

        /// <summary>
        /// 修改模块信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool UpdateModuleInfo(TB_ModuleConfiguration moduleInfo)
        {
            bool b = moduleConfigurationDAL.UpdateModuleInfo(moduleInfo);
            return b;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool InsertModuleInfo(TB_ModuleConfiguration moduleInfo)
        {
            return moduleConfigurationDAL.InsertModuleInfo(moduleInfo);
        }
    }
}
