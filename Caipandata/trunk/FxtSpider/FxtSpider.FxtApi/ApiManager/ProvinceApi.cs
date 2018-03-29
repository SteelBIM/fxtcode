using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.Fxt.Api;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class ProvinceApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ProvinceApi));
        public static FxtApi_SYSProvince GetProvinceById(int fxtId, FxtAPIClientExtend _fxtApi = null)
        {
            FxtApi_SYSProvince obj = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProvinceById";
                var para = new { provinceId = fxtId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                obj = FxtApi_SYSProvince.ConvertToObj(jsonStr);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetProvinceById(int Id:{0})", fxtId), ex);
            }
            return obj;
        }
        public static List<FxtApi_SYSProvince> GetAllProvince(FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSProvince> list = new List<FxtApi_SYSProvince>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProvince";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, "", _fxtApi: fxtApi));

                FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(jsonStr);
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSProvince>();
                }
                list = FxtApi_SYSProvince.ConvertToObjList(Convert.ToString(result.data));
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetAllProvince()", ex);
            }
            return list;
        }
    }
}
