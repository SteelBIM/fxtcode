using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.DAL;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.IBS.IBLL;

namespace CBSS.IBS.BLL
{
    /// <summary>
    /// Cfgmanager接口实现
    /// </summary>
    public partial class IBSService : IIBSService
    {
        Repository repository = new Repository("IBS");
        Repository repository2 = new Repository("IBS2");
        Repository resources = new Repository("Resources");
    }
}
