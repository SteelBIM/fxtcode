using CBSS.Web.API.Controllers;
using CBSS.Framework.Contract.API;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Core.Log;
using CBSS.Web.API.Common;
#pragma warning disable CS0105 // “CBSS.Core.Utility”的 using 指令以前在此命名空间中出现过
using CBSS.Core.Utility;
#pragma warning restore CS0105 // “CBSS.Core.Utility”的 using 指令以前在此命名空间中出现过
using CBSS.Framework.Contract.Enums;
using CourseActivate.Web.API.Filter;

namespace CBSS.Web.API
{
    /// <summary>
    /// CBSS通用接口服务
    /// </summary>
    //[ThrottleServiceBehavior]
    //[CompressAttribute]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CBSSService : ICBSSService
    {
        /// <summary>
        /// 接口入口
        /// </summary>
        /// <param name="requestdate">请求参数</param>
        /// <returns></returns>
        public APIResponse Entrance(APIRequest requestdate)
        {
            string Key = requestdate.Key;
            string Info = requestdate.Info;
            string FunName = requestdate.FunName;
            int FunWay = requestdate.FunWay;
            APIRequestFlag Flag = requestdate.Flag.ToObject<APIRequestFlag>();

            #region 请求值校验
            string requestInfoStr = "";
            APIData apidate = new APIData();
            APIResponse requestdata = apidate.JudgeRequestData(Key, Info, FunName, FunWay);
            if (requestdata.Success)
            {
                requestInfoStr = requestdata.Data.ToString();
            }
            else
            {
                return requestdata;
            }
            #endregion

            try
            {
                APIRequestInfo requestinfo = JsonConvertHelper.FromJson<APIRequestInfo>(requestInfoStr);
                if (requestinfo != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(requestinfo.RTime));
                    if (ts.TotalSeconds > int.Parse(XMLHelper.GetAppSetting("RTimeValidSeconds")))
                    {
                        return APIResponse.GetErrorResponse(ErrorCodeEnum.请求接口失效);
                    }
                    #endregion

                    #region 接口方法体
                    APIResponse responseinfo;
                    Type objectCon = typeof(BaseController);
                    MethodInfo methInfo = objectCon.GetMethod(FunName);
                    if (methInfo == null)
                    {
                        return APIResponse.GetErrorResponse(ErrorCodeEnum.接口方法名不正确, LogLevelEnum.Error);
                    }
                    object[] objinfo;
                    objinfo = new object[] { requestInfoStr};
                    responseinfo = (APIResponse)methInfo.Invoke(null, objinfo);
                    #endregion

                    #region 接口返回值处理
                    if (responseinfo.Success)
                    {
                        return apidate.HandleResponseData(requestinfo.PKey, responseinfo.Data, FunWay);
                    }
                    else
                    {
                        return responseinfo;
                    }
                    #endregion
                }
                else
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.接口请求异常, LogLevelEnum.Error, ex);
            }
        }
    }
}