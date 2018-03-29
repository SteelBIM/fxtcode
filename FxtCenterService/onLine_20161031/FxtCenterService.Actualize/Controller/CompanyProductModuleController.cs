using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 判断是否开通产品模块权限
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string IsAllowCompanyProductModule(JObject funinfo, UserCheck company, JObject objSinfo, JObject objInfo)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int module = StringHelper.TryGetInt(funinfo.Value<string>("module"));
            int cnt = CompanyProductModuleBL.IsAllowCompanyProductModule(search, module, objSinfo, objInfo);
            cnt = cnt > 0 ? 1 : 0;
            var isAllow = new { isAllow = cnt };
            return isAllow.ToJson();
        }
    }
}
