using CAS.Common;
using CAS.Entity;
using FxtCenterService.DataAccess.TempProject;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        static DataController()
        {
            CAS.DataAccess.DA.BaseDA.SetConnectionName<TempFinanceTransfer>("TempDataConnectionString");
        }

        public static string AddTransfer(JObject funinfo, UserCheck company)
        {
            List<TempFinanceTransfer> li = funinfo.Property("addlist") == null ? new List<TempFinanceTransfer>() : JSONHelper.JSONStringToList<TempFinanceTransfer>(funinfo.Property("addlist").Value.ToString());
            int actionResult = 0;
            foreach (TempFinanceTransfer item in li)
            {
                if (TempFinanceTransferDA.Add(item) > 0)
                    actionResult++;
            }
            return actionResult.ToString();
        }
    }
}
