using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Cfgmanager.Contract.DataModel;
using CBSS.Framework.DAL;
using System.Transactions;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.Cfgmanager.IBLL;

namespace CBSS.Cfgmanager.BLL
{
    /// <summary>
    /// Cfgmanager接口实现
    /// </summary>
    public partial class CfgmanagerService : ICfgmanagerService
    {
        Repository repository = new Repository("Cfgmanager");
        Repository repositoryLog = new Repository("Log");
    }
}
