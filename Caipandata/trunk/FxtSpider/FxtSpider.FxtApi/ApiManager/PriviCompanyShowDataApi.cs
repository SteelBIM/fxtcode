using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.FxtApiClientManager;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class PriviCompanyShowDataApi
    {
        public static FxtApi_PriviCompanyShowData GetPriviCompanyShowDataByCompanyIdAndCityId(int fxtCompanyId, int fxtCityId, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            string name = "GetPriviCompanyShowDataByCompanyIdAndCityId";
            var para = new
            {
                fxtCompanyId = fxtCompanyId,
                cityId = fxtCityId
            };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
            fxtApi.Abort();
            FxtApi_PublicResult resultObj = FxtApi_PublicResult.ConvertToObj(jsonStr);
            if (resultObj == null || string.IsNullOrEmpty(Convert.ToString(resultObj.data)))
            {
                return null;
            }
            FxtApi_PriviCompanyShowData obj = FxtApi_PriviCompanyShowData.ConvertToObj(Convert.ToString(resultObj.data));

            return obj;
        }
        /// <summary>
        /// 获取房讯通companyId=25的可查询企业范围
        /// </summary>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static string GetFxtPriviCompanyShowDataCaseCompanyIds(string cityName,FxtAPIClientExtend _fxtApi = null)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return "25";
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            FxtApi_SYSCity city = CityApi.GetCityByCityName(cityName, _fxtApi: fxtApi);
            if (city == null)
            {
                fxtApi.Abort();
                return "25";
            }
            FxtApi_PriviCompanyShowData obj = GetPriviCompanyShowDataByCompanyIdAndCityId(25, city.CityId, _fxtApi: fxtApi);
            fxtApi.Abort();
            if (obj == null || string.IsNullOrEmpty(obj.CaseCompanyId))
            {
                return "25";
            }
            return obj.CaseCompanyId;
        }
        /// <summary>
        /// 获取房讯通companyId=25的可查询企业范围
        /// </summary>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static string GetFxtPriviCompanyShowDataCaseCompanyIds(int fxtCityId,FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            FxtApi_PriviCompanyShowData obj = GetPriviCompanyShowDataByCompanyIdAndCityId(25, fxtCityId, _fxtApi: fxtApi);
            fxtApi.Abort();
            if (obj == null || string.IsNullOrEmpty(obj.CaseCompanyId))
            {
                return "25";
            }
            return obj.CaseCompanyId;
        }
    }
}
