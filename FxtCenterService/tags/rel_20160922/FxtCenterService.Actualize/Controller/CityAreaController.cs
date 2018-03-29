using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using System.Data;
//using FxtOpenClient.ClientService;
//using FxtCommon.Openplatform.GrpcService;
//using FxtCommon.Openplatform.Data;

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
            object areaObj = funinfo.ToString().Contains("areaid")
                ? funinfo.Value<object>("areaid")
                : null;
            int[] areaids = null;
            if (areaObj != null && !string.IsNullOrEmpty(areaObj.ToString()))
                areaids = areaObj.ToString().Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries).Select(StringHelper.TryGetInt).ToArray();

            object zipcodeObj = funinfo.ToString().Contains("zipcode")
                ? funinfo.Value<object>("zipcode")
                : null;
            int[] zipcodes = null;
            if (zipcodeObj != null && !string.IsNullOrEmpty(zipcodeObj.ToString()))
                zipcodes = zipcodeObj.ToString().Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries).Select(StringHelper.TryGetInt).ToArray();

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;

            //added by: dpc, 2015-12-30
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //{
            //    AreaResponse areaResponse;
            //    var requestParam = new AreaRequestParam()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            CompanyId = company.companyid,
            //            CityId = search.CityId
            //        },
            //        AreaIdStr = areaObj.ToString(),
            //        ZipCodeStr = zipcodeObj.ToString()
            //    };
            //    FxtClientService.GetSysAreaList(requestParam, out areaResponse);

            //    return areaResponse.SysAreas.ToLowerJson();
            //}


            List<SYSArea> arealist = SYSAreaBL.GetSYSAreaList(search, areaids, zipcodes);
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
            //added by: dpc
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //{
            //    ProvinceResponse provinceResponse;
            //    var request = new ProvinceRequestParam()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            CompanyId = company.companyid,
            //        }
            //    };


            //    FxtClientService.GetSysProvinceList(request, out provinceResponse);

            //    return provinceResponse.SysProvinces.ToLowerJson();
            //}


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
            int provinceid = funinfo.ToString().Contains("provinceid")
                ? StringHelper.TryGetInt(funinfo.Value<object>("provinceid").ToString())
                : 0;

            int zipcode = funinfo.ToString().Contains("zipcode")
                ? StringHelper.TryGetInt(funinfo.Value<object>("zipcode").ToString())
                : 0;

            //added by: dpc
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //{
            //    CityResponse cityResponse;
            //    var request = new CityRequestParam()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            CompanyId = company.companyid
            //        },
            //        ProvinceId = provinceid,
            //        Zipcode = zipcode
            //    };

            //    FxtClientService.GetSysCityList(request, out cityResponse);
            //    return cityResponse.SysCitys.ToLowerJson();
            //}


            List<SYSCity> citylist = SYSAreaBL.GetSYSCityList(provinceid, zipcode);
            return citylist.ToJson();
        }

        /// <summary>
        /// 城市列表(根据省份zipcode)
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSYSCityListByPZipCode(JObject funinfo, UserCheck company)
        {
            int zipcode = funinfo.ToString().Contains("zipcode")
                ? StringHelper.TryGetInt(funinfo.Value<object>("zipcode").ToString())
                : 0;
            List<SYSCity> citylist = SYSAreaBL.GetSYSCityListByPZipCode(zipcode);
            return citylist.ToJson();
        }

        /// <summary>
        /// 片区列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSubAreaList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            //search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            List<SYSSubArea> arealist = SYSAreaBL.GetSubAreaList(search, areaId);
            return arealist.ToJson();
        }

        /// <summary>
        /// 商业片区列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSubAreaListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            List<SYSSubAreaBiz> arealist = SYSAreaBL.GetSubAreaListBiz(search, areaId);
            return arealist.ToJson();
        }
        /// <summary>
        /// 办公片区列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSubAreaListOffice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            List<SYSSubAreaOffice> arealist = SYSAreaBL.GetSubAreaListOffice(search, areaId);
            return arealist.ToJson();
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCityInfo(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int cityid = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            string cityname = funinfo.Value<string>("cityname");
            string re = SYSCityBL.GetSYSCityInfo(search, cityid, cityname);
            return re;
        }

        /// <summary>
        /// 获取行政区信息
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetAreaInfo(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            string re = SYSCityBL.GetSYSAreaInfo(search, areaid);
            return re;
        }

        /// <summary>
        /// 获取公司开通的城市列表
        /// 由于需要调用用户中心的接口,添加了两个导入参数---饶成龙[2016-09-12]
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSYSCityListByCompany(JObject funinfo, UserCheck company, JObject objSinfo, JObject objInfo)
        {
            List<SYSCity> citylist = SYSAreaBL.GetSYSCityListByCompany(objSinfo, objInfo,company.signname, company.producttypecode);
            return citylist.ToJson();
        }
    }
}
