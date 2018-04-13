using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.IDAL;

namespace Kingsun.IBS.BLL.IBSLearningReport
{
    public class IBSInitStuCatalogBLL : IIBSInitStuCatalogBLL
    {
        private IIBSInitStuCatalogDAL dal = new IBSInitStuCatalogDAL();
        public void InitializeUserInfo(string connectionstring)
        {
            dal.InitializeUserInfo(connectionstring);
        }


        public void TodayInitializeUserInfo(string connectionstring)
        {
            dal.TodayInitializeUserInfo(connectionstring);
        }
    }
}
