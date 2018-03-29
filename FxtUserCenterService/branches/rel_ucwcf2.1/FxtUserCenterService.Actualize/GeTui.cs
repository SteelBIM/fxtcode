using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Contract;
using System.ServiceModel.Activation;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Common;
using FxtUserCenterService.Actualize.Impl;

namespace FxtUserCenterService.Actualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GeTui : IGeTui
    {
        /// <summary>
        /// 个推入口
        /// </summary>
        /// <param name="certifyargs"></param>
        /// <param name="funargs"></param>
        /// <returns></returns>
        public WCFJsonData GeTuiSend(string funinfo)
        {
            WCFJsonData jsonData = new WCFJsonData();
            try
            {//调用
                JObject objinfo = JObject.Parse(funinfo);
                string token = objinfo.Value<string>("token");
                string time = objinfo.Value<string>("time");
                string tokenVal = EncryptHelper.GetMd5(time, "getui@yck2014");
                if (objinfo.Value<string>("funname") == "mpgttwo" && token == tokenVal)
                {
                    jsonData = new Implement().GeTuiSend(funinfo);
                }
                else
                {
                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "非法访问");                    
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }
    }
}
