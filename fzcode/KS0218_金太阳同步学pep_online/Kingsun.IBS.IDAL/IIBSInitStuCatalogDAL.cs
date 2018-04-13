using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IDAL
{
    public interface IIBSInitStuCatalogDAL
    {

        bool InitializeUserInfo(string connectionstring);
        bool TodayInitializeUserInfo(string connectionstring);
    }
}
