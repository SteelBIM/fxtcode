using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class ModularSortDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        /// 查询所有TB_ModuleSort
        /// </summary>
        /// <returns></returns>
        public IList<TB_ModuleSort> GetModuleList()
        {
            IList<TB_ModuleSort> list = SelectAll<TB_ModuleSort>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleSort> GetModuleList(string where)
        {
            IList<TB_ModuleSort> list = Search<TB_ModuleSort>(where);
            return list;
        }
    }
}
