using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IAppSkinVersionService
    {
        IEnumerable<V_AppSkinVersion> GetAppSkinVersionList(out int totalcount, AppVersionRequest request = null);
        AppSkinVersion GetAppSkinVersion(int id);
        void SaveAppSkinVersion(AppSkinVersion model);
    }
}
