using CBSS.Cfgmanager.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.IBLL
{
    /// <summary>
    /// Cfgmanager功能接口注册
    /// </summary>
    public interface ICfgmanagerService : IApiFunctionService, IDBConfigService,IDictItemService, ILog4netService
    {       
    }
}
