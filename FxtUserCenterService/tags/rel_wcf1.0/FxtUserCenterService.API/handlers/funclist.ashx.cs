using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtUserCenterService.Entity;
using FxtUserCenterService.Logic;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// company 的摘要说明
    /// </summary>
    public class funclist : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "appid", "apppwd", "signname", "splatype" })) return;

            var appid = GetRequest("appid");
            var apppwd = GetRequest("apppwd");
            var signname = GetRequest("signname");
            var platType = GetRequest("splatype"); 

            string result = string.Empty;

            try
            {
                var companyProAppFunc = CompanyProductAppFuncBL.GetList(appid, apppwd, signname, platType);

                if (companyProAppFunc == null || companyProAppFunc.Count() == 0)
                {
                    result = GetJson(0, "找不到对应的功能清单");
                }

                var funlist = new List<dynamic>();

                companyProAppFunc.ToList().ForEach(m => funlist.Add(
                    new
                    {

                        funid = m.funid,
                        istrue = m.valid,
                        callbackurl = m.callbackurl,
                        callbackformat = m.callbackformat

                    }));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = GetJson(ex);
            }
            context.Response.Write(result);
        }

    }
}