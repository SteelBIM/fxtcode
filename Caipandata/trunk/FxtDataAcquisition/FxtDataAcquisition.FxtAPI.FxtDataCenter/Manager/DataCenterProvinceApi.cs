using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public class DataCenterProvinceApi
    {
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSProvince> GetProvinceAll(string username, string signname, List<Apps> appList)
        {
            var para = new { };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.provincelist, para, appList);
            List<FxtApi_SYSProvince> list = new List<FxtApi_SYSProvince>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<FxtApi_SYSProvince>();
            }
            return list;
        }
    }
}
