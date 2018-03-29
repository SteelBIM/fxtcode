using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        /// <summary>
        /// 对外方法名：cptcityids 参数名：signname,productcode(创建人:谭启龙)
        /// 20160308 获取公司开通产品模块的城市ID
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductModuleCityIds(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            string signname = Convert.ToString(func["signname"]);
            var productcode = StringHelper.TryGetInt(func["productcode"].ToString());

            if (string.IsNullOrEmpty(signname))
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");    //companyid传入必须是个大于0的值
            }
            var list = CompanyProductModuleBL.GetList(signname, productcode);

            if (0 < list.Count)
            {
                var temp = list.Select(query => Convert.ToInt32(query.cityid)).ToArray();
                return JSONHelper.GetWcfJson(temp, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");
            }
        }

        /// <summary>
        /// 查询是否开通产品模块权限
        /// zhoub 20160908
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductModuleWhetherToOpen(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            int producttypecode =StringHelper.TryGetInt(func["producttypecode"].ToString());
            int cityid = StringHelper.TryGetInt(func["cityid"].ToString());
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            int parentmodulecode = StringHelper.TryGetInt(func["parentmodulecode"].ToString());
            int modulecode = StringHelper.TryGetInt(func["modulecode"].ToString());

            var list = CompanyProductModuleBL.GetCompanyProductModuleWhetherToOpen(producttypecode, cityid, companyid, parentmodulecode, modulecode).Select(s => new
            {
                id = s.id,
                companyid = s.companyid
            }).ToList();

            if (list.Count > 0)
            {
                return JSONHelper.GetWcfJson(list, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null,(int)EnumHelper.Status.Success, "未查询到任何数据");
            }
        }


        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块城市ID(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductAndCompanyProductModuleCityIds(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            int producttypecode = StringHelper.TryGetInt(func["producttypecode"].ToString());
            var search = func["search"].ToString();
            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var companyList = CompanyProductModuleBL.GetCompanyProductAndCompanyProductModuleCityIds(SearchBase,companyid, producttypecode).Select(s => new
            {
                cityid = s.cityid,
                recordcount=s.recordcount
            });

            return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
        }

        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块详细信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductModuleDetails(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            int producttypecode = StringHelper.TryGetInt(func["producttypecode"].ToString());
            var search = func["search"].ToString();
            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var companyList = CompanyProductModuleBL.GetCompanyProductModuleDetails(SearchBase,companyid, producttypecode).Select(s => new
            {
                cityid = s.cityid,
                createdate = s.createdate,
                companyid = s.companyid,
                producttypecode = s.producttypecode,
                modulecode = s.modulecode,
                parentmodulecode = s.parentmodulecode,
                recordcount=s.recordcount
            });

            return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
        }
    }
}
