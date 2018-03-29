using CAS.Entity;
using CAS.Entity.DBEntity;
using FxtCenterService.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using CAS.Entity.FxtProject;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 新增公司
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string AddPriviCompanyForVQ(JObject funinfo, UserCheck company)
        {
            Privi_Company model = funinfo.ToObject<Privi_Company>();
            if (string.IsNullOrEmpty(model.companyname))
            {
                return null;
            }
            if (string.IsNullOrEmpty(model.companycode))
            {
                return null;
            }
            return PriviCompanyBL.AddPriviCompanyForVQ(model).ToJson();
        }
    }
}
