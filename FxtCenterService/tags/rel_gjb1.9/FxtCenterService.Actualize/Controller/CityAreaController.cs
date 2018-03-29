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
        /// 区域列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSYSAreaList(JObject funinfo, UserCheck company)
        {
            object areaObj = funinfo.Value<object>("areaid");
            int[] areaids = null;
            if(areaObj!=null && !string.IsNullOrEmpty(areaObj.ToString()))
                areaids = areaObj.ToString().Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries).Select(StringHelper.TryGetInt).ToArray();

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            List<SYSArea> arealist = SYSAreaBL.GetSYSAreaList(search, areaids);
            return arealist.ToJson();
        }


        /// <summary>
        /// 省份列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetProvinceList(JObject funinfo, UserCheck company)
        {
            List<SYSProvince> provincelist = SYSAreaBL.GetProvinceList();
            return provincelist.ToJson();
        }

        /// <summary>
        /// 城市列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSYSCityList(JObject funinfo, UserCheck company)
        {
            int provinceid = StringHelper.TryGetInt(funinfo.Value<object>("provinceid").ToString());
            List<SYSCity> citylist = SYSAreaBL.GetSYSCityList(provinceid);
            return citylist.ToJson();
        }




    }
}
