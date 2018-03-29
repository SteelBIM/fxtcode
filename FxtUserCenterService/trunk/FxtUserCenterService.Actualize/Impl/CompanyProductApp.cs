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
        /// <summary>
        /// productapiinfo 通过Fxtcompanyid=25来取别的公司的ProductAPI
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetProductAPIInfo(string sinfo, string info)
        {

            
            var signname = JObject.Parse(sinfo)["signname"].ToString();

            int systypecode = StringHelper.TryGetInt(JObject.Parse(info)["appinfo"]["systypecode"].ToString());

            var func = JObject.Parse(info)["funinfo"];
            string othersignname =func["othersignname"].ToString();//
            var appid = StringHelper.TryGetInt(func["appid"].ToString());
            if (!string.IsNullOrEmpty(othersignname))
            {
                signname = othersignname;
            }



            if (systypecode==0)
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数");    //companyid传入必须是个大于0的值

            var list = CompanyProductAppBL.GetProductAPIInfo(appid, systypecode, signname);
      
            if (0 < list.Count)
                return JSONHelper.GetWcfJson(list, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "此产品没有配置可使用的Api");
        }
    }
}
