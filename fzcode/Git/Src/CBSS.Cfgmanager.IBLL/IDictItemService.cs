using CBSS.Cfgmanager.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.IBLL
{
    public interface IDictItemService
    {
        Sys_DictItem GetDictItem(int DictItemID);
        List<Sys_DictItem> GetDictItemList(string DictItemName, int SystemCode);
        IEnumerable<Sys_DictItem> GetAllSys_DictItem(out int totalcount, ApiFunctionRequest request = null);
        bool DeleteDictItem(List<int> ids);
        void SaveDictItem(Sys_DictItem model);
    }
}
