using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using FxtCenterService.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FxtCenterService.Logic
{
    public class SYSAreaBL
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSArea> GetSYSAreaList(SearchBase search, int[] areaid, string zipcodeStr)
        {
            string areaidstr = (areaid == null || areaid.Length == 0) ? null : string.Join(",", areaid.Select(i => i.ToString()).ToArray());
            return SYSAreaDA.GetSYSAreaList(search, areaidstr, zipcodeStr);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityList(int provinceid, int zipcode)
        {
            return SYSAreaDA.GetSYSCityList(provinceid, zipcode);
        }
        /// <summary>
        /// 获取城市列表(根据省份zipcode)
        /// </summary>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityListByPZipCode(int zipcode)
        {
            return SYSAreaDA.GetSYSCityListByPZipCode(zipcode);
        }
        /// <summary>
        /// 获取区域
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static SYSArea GetSYSArea(int areaId)
        {
            return SYSAreaDA.GetSYSAreaById(areaId);
        }
        /// 获取省份列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSProvince> GetProvinceList()
        {
            return SYSAreaDA.GetProvinceList();
        }

        /// <summary>
        /// 获取片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSSubArea> GetSubAreaList(SearchBase search, int areaId)
        {
            return SYSAreaDA.GetSubAreaList(search, areaId);
        }
        /// <summary>
        /// 获取商业片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSSubAreaBiz> GetSubAreaListBiz(SearchBase search, int areaId)
        {
            return SYSAreaDA.GetSubAreaListBiz(search, areaId);
        }
        /// <summary>
        /// 获取办公片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSSubAreaOffice> GetSubAreaListOffice(SearchBase search, int areaId)
        {
            return SYSAreaDA.GetSubAreaListOffice(search, areaId);
        }

        /// <summary>
        /// 获取公司开通的城市列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityListByCompany(JObject objSinfo, JObject objInfo ,string signname, int productcode)
        {
            string returnStr = FxtCenterWebCommon.UserCenterApiPost(FxtCenterWebCommon.UserCenterApiPostData(objSinfo, objInfo["appinfo"]["systypecode"].ToString(), "cptmcityids", new
            {
                signname = signname,
                productcode = productcode,
                valid = 1
            }));
            JObject returnJson = JObject.Parse(returnStr);
            if (returnJson.Value<int>("returntype") == -1 && returnJson["returntext"].ToString() == "找不到机构")
            {
                return new List<SYSCity>();
            }
            else if (returnJson.Value<int>("returntype") != 1)
            {
                throw new Exception(string.Format("调用接口异常,返回信息:{0},状态值:{1}", returnJson.Value<string>("returntext"), returnJson.Value<string>("returntype")));
            }
            List<int> cityIDs = JsonConvert.DeserializeObject<int[]>(returnJson["data"].ToString()).ToList();
            return SYSAreaDA.GetSYSCityListByID(cityIDs);
        }
    }
}
