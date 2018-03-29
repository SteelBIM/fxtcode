using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        //对外方法名：cp_func_1 参数名：companyid,producttypecode,signname
        public WCFJsonData GetCompanyProduct(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var companyid = func["companyid"].ToString();
            var producttypecode = func["producttypecode"].ToString();
            var signname = func["signname"].ToString();

            var cproduct = CompanyProductBL.GetInfo(int.Parse(companyid), int.Parse(producttypecode), signname, 1);

            if (cproduct!=null)
                return JSONHelper.GetWcfJson(cproduct, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");
        }
    }
}
