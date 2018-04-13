using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL
{
    public interface IIBSInitStuCatalogBLL
    {
        void InitializeUserInfo(string conenction);

        void TodayInitializeUserInfo(string conenction);
    }
}
