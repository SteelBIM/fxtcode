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
    public class company : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "type" })) return;
            string result = "";
            string companycode = GetRequest("companycode");
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            int productTypeCode = StringHelper.TryGetInt(GetRequest("producttypecode"));
            string key = GetRequest("key");
            string type = GetRequest("type");
            string signname = GetRequest("signname");
            CompanyInfo company = null;
            try
            {
                switch (type)
                {
                    case "signname":
                        company = CompanyBL.GetCompanyBySignName(signname);
                         if (company != null) result = GetJson(company, 1, "");
                        else result = GetJson(0, "找不到此机构");
                        break;
                    case "companycode":
                        company = CompanyBL.Get(companycode);
                        if (company != null) result = GetJson(company, 1, "");
                        else result = GetJson(0, "找不到此机构");
                        break;
                    case "companyid":
                        company = CompanyBL.Get(companyid);
                        if (company != null) result = GetJson(company, 1, "");
                        else result = GetJson(0, "找不到此机构");
                        break;
                    case "editcompany":
                        string wxid = GetRequest("wxid");
                        string wxname = GetRequest("wxname");
                        int index = CompanyBL.update(companycode, wxid, wxname);
                        if (index > 0) result = GetJson(1, "success");
                        else result = GetJson(0, "微信公众添加失败!");
                        break;
                    case "companysearch"://搜索公司。

                        var companyList = CompanyBL.GetCompanyInfo(search, productTypeCode, key).Select(o => new
                        {
                            wxid = o.wxid,
                            companyid = o.companyid,
                            companycode = o.companycode,
                            companyname = o.companyname,
                            overdate = o.OverDate,
                            recordcount = o.recordcount
                        });
                        result = GetJson(companyList, 1, "success");
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