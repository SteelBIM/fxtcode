using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.Common
{
    public class FxtApiCommon
    {
        /// <summary>
        /// 根据获取指定apicode的api信息
        /// </summary>
        /// <param name="nowApiCode">api的Code</param>
        /// <param name="appList">所有api信息集合</param>
        /// <param name="appUrl"></param>
        /// <param name="appPwd"></param>
        /// <param name="appKey"></param>
        public static void GetNowApiInfo(int nowApiCode,List<Apps> appList, out string appUrl, out string appPwd, out string appKey)
        {
            appUrl = ""; appPwd = ""; appKey = "";
            if (appList != null)
            {
                Apps appInfo = appList.Where(obj => obj.AppId == nowApiCode).FirstOrDefault();
                if (appInfo != null)
                {
                    appUrl = appInfo.AppUrl;
                    appPwd = appInfo.AppPwd;
                    appKey = appInfo.AppKey;
                }
            }
        }
    }
}
