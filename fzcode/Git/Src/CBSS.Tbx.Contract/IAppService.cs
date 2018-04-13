using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Tbx.Contract
{
    public interface IAppService
    {
        void SaveApp(App model); 
        IEnumerable<App> GetAppListByStatus();
        IEnumerable<App> GetAppList(out int totalaount, AppRequest request = null);
        App GetApp(int id);

        void DeleteApp(List<int> ids);
    }
}
