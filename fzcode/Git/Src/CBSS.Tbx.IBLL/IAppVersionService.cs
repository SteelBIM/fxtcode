using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
   public interface IAppVersionService
    {
        IEnumerable<v_AppVersion> GetAppVersionList(out int totalcount, AppVersionRequest request = null);
        AppVersion GetAppVersion(int id);
        int SaveAppVersion(AppVersion model);
        bool DelAppVersion(int AppVersionID);
        IEnumerable<AppVersion> GetAppVersion(Expression<Func<AppVersion, bool>> expression);
    }
}
