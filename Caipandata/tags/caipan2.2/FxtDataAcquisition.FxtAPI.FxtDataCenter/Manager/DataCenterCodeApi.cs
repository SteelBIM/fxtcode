using FxtDataAcquisition.NHibernate.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public static class DataCenterCodeApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static List<SYSCode> GetCodeById(int id, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { id = id };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.gscodelist, para,  appList);
            List<SYSCode> list = new List<SYSCode>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<SYSCode>();
            }
            return list;
        }
    }
}
