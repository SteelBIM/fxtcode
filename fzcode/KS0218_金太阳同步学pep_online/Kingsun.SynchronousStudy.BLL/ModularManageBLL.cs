using System.Collections.Generic;
using Kingsun.SynchronousStudy.BLL.Contract;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    /// <summary>
    /// IndustryBLL 的摘要说明
    /// </summary>
    public class ModularManageBLL : IModularManageBLL
    {
        private readonly ModularManageDAL modularManageDAL = new ModularManageDAL();

        public List<TB_ModularManage> GetModularList()
        {
            return modularManageDAL.GetModularList();
        }

        public IList<TB_ModularManage> GetModuleList(string where)
        {
            return modularManageDAL.GetModuleList(where);
        }
    }
}