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
    /// companyproduct 的摘要说明
    /// </summary>
    public class companyproduct : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "type", "producttypecode" })) return;
            string result = "";
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));//公司ID
            string signname = GetRequest("signname");//公司标识
            int producttypecode = StringHelper.TryGetInt(GetRequest("producttypecode"));
            string type = GetRequest("type");
            string strcode = GetSysCode(GetRequest("strdate"));
            CompanyProduct cproduct = null;
            try
            {
                switch (type)
                {
                    case "companyproduct"://根据公司id和产品code获取信息(caoq 2013-7-12)
                        cproduct = CompanyProductBL.GetInfo(companyid, producttypecode, signname, 1);
                        if (cproduct != null) result = GetJson(cproduct, 1, "");
                        else result = GetJson(0, "找不到机构");
                        break;
                }
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