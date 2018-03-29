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
    public class DataCenterCityApi
    {
        /// <summary>
        /// 获取行政区列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSCity> GetCityAll(string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new {};
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.citylist, para, appList);
            List<FxtApi_SYSCity> list = new List<FxtApi_SYSCity>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<FxtApi_SYSCity>();
            }
            return list;
        }
    }
}
