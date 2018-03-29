using CAS.Common;
using CAS.Entity;
using FxtCenterService.Logic;
using Newtonsoft.Json.Linq;
using OpenPlatform.Framework.FlowMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 浦发楼盘获取接口
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectList_SPDB(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");//楼盘关键字
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            var list = SPDBBL.GetProjectList(search, key);
            return list.ToJson();
        }

        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaselist_SPDB(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));//楼盘ID
            if (projectid <= 0)
            {
                return "";
            }
            decimal buildingarea = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingarea"));//房屋面积
            if (buildingarea <= 0)
            {
                return "";
            }
            if (funinfo.Property("casetypecode") == null || !(funinfo.Property("casetypecode").Value.ToString() == "0" || funinfo.Property("casetypecode").Value.ToString() == "1"))
            {
                return "";
            }
            int casetypecode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));//案例类型Code
            string purposecodeStr = funinfo.Property("purposecode") == null ? "" : funinfo.Value<string>("purposecode");
            List<int> purposecodes = purposecodeStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(o => StringHelper.TryGetInt(o)).ToList();//案例用途Code
            if (purposecodes.Count <= 0 && casetypecode == 1)
            {
                return "";
            }
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));//所在楼层
            string houseno = funinfo.Value<string>("houseno");//楼房号
            int buildingtypecode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));//建筑类型
            int housetypecode = StringHelper.TryGetInt(funinfo.Value<string>("housetypecode"));//户型
            return SPDBBL.GetProjectCaseList(search, projectid,
                buildingarea,
                purposecodes,
                casetypecode,
                floorno,
                houseno,
                buildingtypecode,
                housetypecode
            );
        }
    }
}
