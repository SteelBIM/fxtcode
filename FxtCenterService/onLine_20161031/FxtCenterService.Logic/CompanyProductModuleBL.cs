using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtUserCenter;
using Newtonsoft.Json.Linq;
using FxtCenterService.Common;

namespace FxtCenterService.Logic
{
    public class CompanyProductModuleBL
    {
        /// <summary>
        /// 判断是否开通产品模块权限
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static int IsAllowCompanyProductModule(SearchBase search, int module, JObject objSinfo, JObject objInfo)
        {
            int result;
            string returnStr = FxtCenterWebCommon.UserCenterApiPost(FxtCenterWebCommon.UserCenterApiPostData(objSinfo, objInfo["appinfo"]["systypecode"].ToString(), "cpmone", new
            {
                producttypecode = search.SysTypeCode,
                cityid = search.CityId,
                companyid = search.FxtCompanyId,
                parentmodulecode = 0,
                modulecode = module
            }));
            JObject returnJson = JObject.Parse(returnStr);
            if (returnJson.Value<int>("returntype") != 1)
            {
                throw new Exception(string.Format("调用接口异常,返回信息:{0},状态值:{1}", returnJson.Value<string>("returntext"), returnJson.Value<string>("returntype")));
            }
            JArray returnData = JArray.Parse(returnJson["data"].ToString() == "null" ? "[]" : returnJson["data"].ToString());
            result = returnData.Count;
            return result;
        }

        public static CompanyProduct IsCompanyProductCity(int conpanyId, int systypecode, int cityId, JObject objSinfo, JObject objInfo)
        {
            CompanyProduct result = new CompanyProduct();
            string returnStr = FxtCenterWebCommon.UserCenterApiPost(FxtCenterWebCommon.UserCenterApiPostData(objSinfo, objInfo["appinfo"]["systypecode"].ToString(), "companythirteen", new
            {
                companyid = conpanyId,
                producttypecode = systypecode,
                cityid = cityId
            }));
            JObject returnJson = JObject.Parse(returnStr);
            if (returnJson.Value<int>("returntype") != 1)
            {
                throw new Exception(string.Format("调用接口异常,返回信息:{0},状态值:{1}", returnJson.Value<string>("returntext"), returnJson.Value<string>("returntype")));
            }
            JArray returnDatas = JArray.Parse(returnJson["data"].ToString());
            if (returnDatas.Count == 0)
            {
                return null;
            }
            else
            {
                int intValue;
                if (int.TryParse(returnDatas[0]["parentproducttypecode"].ToString(), out intValue))
                {
                    result.parentproducttypecode = intValue;
                }
                if (int.TryParse(returnDatas[0]["parentshowdatacompanyid"].ToString(), out intValue))
                {
                    result.parentshowdatacompanyid = intValue;
                }
            }
            return result;
        }
    }
}
