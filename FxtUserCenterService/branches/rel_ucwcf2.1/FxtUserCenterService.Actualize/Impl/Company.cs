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

        //对外方法名：company_func_5 参数名：signname
        public WCFJsonData GetCompanyBySignName(string sinfo, string info)
        {
            var sInfo = JObject.Parse(sinfo);

            var signName = sInfo["signname"].ToString();

            var company = CompanyBL.GetCompanyBySignName(signName);

            if (company != null)
                return JSONHelper.GetWcfJson(company, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到此机构");
        }

        //对外方法名：company_func_1 参数名：companycode
        public WCFJsonData GetCompanyByCode(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var companycode = func["companycode"].ToString();

            var company = CompanyBL.Get(companycode);

            if (company != null)
                return JSONHelper.GetWcfJson(company, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到此机构");
        }

        //对外方法名：company_func_2 参数名：companyid
        public WCFJsonData GetCompanyById(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var companyid = func["companyid"].ToString();

            var company = CompanyBL.Get(int.Parse(companyid));

            if (company != null)
                return JSONHelper.GetWcfJson(company, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到此机构");
        }


        //对外方法名：company_func_3 参数名：companycode,wxid,wxname
        public WCFJsonData CompanyEdit(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var companycode = func["companycode"].ToString();
            var wxid = func["wxid"].ToString();
            var wxname = func["wxname"].ToString();

            int index = CompanyBL.update(companycode, wxid, wxname);

            if (index > 0)
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "微信公众号添加失败");
        }

        //对外方法名：company_func_4 参数名：search ,productTypeCode,key
        public WCFJsonData GetCompanyBySearch(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var search = func["search"].ToString();
            var productTypeCode = int.Parse(func["productTypeCode"].ToString());
            var key = func["key"].ToString();

            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var companyList = CompanyBL.GetCompanyInfo(SearchBase, productTypeCode, key).Select(o => new
            {
                wxid = o.wxid,
                companyid = o.companyid,
                companycode = o.companycode,
                companyname = o.companyname,
                overdate = o.OverDate,
                recordcount = o.recordcount
            });

            return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
        }


        /// <summary>
        /// 如果通过安全认证，返回参数  代理方法名company_func_6
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyInfoIfSecurity(string sinfo, string info)
        {
            JObject objinfo = JObject.Parse(info);


            JObject appinfo = objinfo["appinfo"] as JObject;
            JObject objSinfo = JObject.Parse(sinfo);
            WCFJsonData jsonData = new WCFJsonData();
            string functionname = objSinfo.Value<string>("functionname");//方法名
            int appid = StringHelper.TryGetInt(objSinfo.Value<string>("appid"));//要调用的接口序列号
            string apppwd = objSinfo.Value<string>("apppwd"); //接口密码
            string time = objSinfo.Value<string>("time");//时间
            string signname = objSinfo.Value<string>("signname");//商户标示号
            string code = objSinfo.Value<string>("code");//时间
            int systypecode = StringHelper.TryGetInt(appinfo.Value<string>("systypecode"));

            CompanyInfo companyInfo = CompanyBL.GetCompanyInfoBySignName(systypecode, signname, appid, apppwd, functionname);
            if (companyInfo != null)
            {
                jsonData = JSONHelper.GetWcfJson(companyInfo, (int)EnumHelper.Status.Success, "成功");

            }
            else
            {
                jsonData = JSONHelper.GetWcfJson(companyInfo, (int)EnumHelper.Status.Failure, "Appid或Apppwd不匹配");
            }
            return jsonData;
        }

        /// <summary>
        /// 获取公司列表 对外方法名：company_func_7 参数名：companyname,companytypecode
        /// caoq 2014-06-19
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyList(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var search = func.Contains("search") ? func["search"].ToString() : "";
            var companytypecode = int.Parse(func["companytypecode"].ToString());
            var companyname = func["companyname"].ToString();

            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var companyList = CompanyBL.GetCompanyList(SearchBase, companyname, companytypecode).Select(o => new
            {
                companyid = o.companyid,
                companycode = o.companycode,
                companyname = o.companyname,
                overdate = o.OverDate,
                recordcount = o.recordcount
            });            
            return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
        }
    }

}
