using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Common;
using FxtUserCenterService.Logic;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {

        //对外方法名：valide_id 参数名：producttypecode,validate ,功能：验证应用身份
        public WCFJsonData ValidateCallIdentity(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            int producttypecode = StringHelper.TryGetInt(func.Value<string>("producttypecode"));
            string validate = func.Value<string>("validate");
            CompanyProductSafe companyProductSafe = CompanyProductSafeBL.ValidateCallIdentity(producttypecode, validate);
            if (companyProductSafe != null)
            {
                var cproduct = new { appkey = companyProductSafe.appkey, companyid = companyProductSafe.companyid, companyname = companyProductSafe.companyname };
                return JSONHelper.GetWcfJson(cproduct, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "未授权");
            }   
        }
    }
}
