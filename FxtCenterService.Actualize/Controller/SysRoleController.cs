using CAS.Entity;
using FxtCenterService.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {

        public static string GetSysRoleUserids(JObject funinfo, UserCheck company)
        {
            string userName = funinfo.Value<string>("username");
            //int cityId = funinfo.Value<int>("cityid");
            List<int> list = SysRoleBL.GetSysRoleUserIds(userName, company.companyid, company.producttypecode);
            return JsonConvert.SerializeObject(list);
        }
    }
}
