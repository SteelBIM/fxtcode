using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class SYSProvinceApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSProvinceApi));
        public static List<FxtApi_SYSProvince> GetAllProvince(FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSProvince> list = new List<FxtApi_SYSProvince>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProvince";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, "", _fxtApi: fxtApi));

                FxtApi_PublicResult result = jsonStr.ParseJSONjss<FxtApi_PublicResult>();
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSProvince>();
                }
                list = Convert.ToString(result.data).ParseJSONList<FxtApi_SYSProvince>();
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
