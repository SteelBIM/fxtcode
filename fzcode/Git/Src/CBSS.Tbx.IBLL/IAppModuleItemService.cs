using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
   public interface IAppModuleItemService
    {
        IEnumerable<v_AppModuleItem> GetAppModuleItemList(out int totalcount, AppModuleItemRequest request = null);
    }
}
