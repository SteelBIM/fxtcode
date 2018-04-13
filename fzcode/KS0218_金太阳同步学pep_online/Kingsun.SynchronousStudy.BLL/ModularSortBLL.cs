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
    public class ModularSortBLL
    {
        ModularSortDAL modularSortDAL = new ModularSortDAL();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<TB_ModuleSort> GetModuleList()
        {
            return modularSortDAL.GetModuleList();
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_ModuleSort> GetModuleList(string where)
        {
            return modularSortDAL.GetModuleList(where);
        }
    }
}
