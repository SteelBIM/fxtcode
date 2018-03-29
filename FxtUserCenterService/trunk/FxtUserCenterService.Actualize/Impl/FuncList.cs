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
        //对外方法名：fl_func_1 参数名：appid，apppwd，signname，platType,producttypecode
        public WCFJsonData GetFunlist(string sinfo, string info)
        {

            var appid = JObject.Parse(sinfo)["appid"].ToString();
            var apppwd = JObject.Parse(sinfo)["apppwd"].ToString();
            var signname = JObject.Parse(sinfo)["signname"].ToString();
            var platType = JObject.Parse(info)["appinfo"]["splatype"].ToString();
            //这里要加上systypecode kevin 20140330
            var systypecode = JObject.Parse(info)["appinfo"]["systypecode"].ToString();
            var companyProAppFunc = CompanyProductAppFuncBL.GetList(appid, apppwd, signname, platType, systypecode);

            if (companyProAppFunc == null || companyProAppFunc.Count() == 0)
            {

                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到对应的功能清单");
            }
            
            return JSONHelper.GetWcfJson(companyProAppFunc.ToList(), (int)EnumHelper.Status.Success, "成功");

        }
    }
}
