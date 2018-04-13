using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System.Web.Configuration;
using System.Web.Http;
using CBSS.IBS.IBLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.API.Model;
using CourseActivate.Web.API.SMSService;
using CBSS.Framework.Redis;
using CBSS.Framework.Contract;
using CBSS.Tbx.BLL;
using CBSS.Framework.Contract.Enums;
using CourseActivate.Web.API.Filter;
namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// app相关
    /// </summary>
    public partial class BaseController
    {
        //
        // GET: /App/        
        //-----------------以下启动相关接口-------------------------
        /// <summary>
        /// 9 获取app版本，查询表 √
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetNewVersions(string inputStr)
        {
            VersionInfo input;
            var verifyResult = tbxService.VerifyParam<VersionInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            string errMsg;
            var appVersion = tbxService.GetVersion(input.appID, input.versionNumber, input.appType, out errMsg);

            if (appVersion == null)
            {
                return APIResponse.GetErrorResponse(errMsg);
            }
            else
            {
                if (appVersion.Count == 1)
                {
                    return APIResponse.GetResponse(appVersion.First());
                }
                else
                {
                    return APIResponse.GetResponse(appVersion);
                }
            }
        }

        /// <summary>
        /// 16 用户使用App时长埋点 △
        /// </summary>
        [HttpPost]
        public static APIResponse SetUserAppRecord(string inputStr)
        {
            UseAppRecord input;
            var verifyResult = tbxService.VerifyParam<UseAppRecord>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            Rds_UseAppRecord record = new Rds_UseAppRecord();
            record.UserID = record.UserID;
            record.UseAppTime = record.UseAppTime;
            record.AppChannelID = record.AppChannelID;
            record.CreateData = DateTime.Now;
            record.AppID = input.AppID;
            record.AppVersionNumber = input.VersionNumber;

            if (!tbxService.SetUseAppRecord(record))
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.用户使用App时长埋点失败, LogLevelEnum.Error);
            }
            return APIResponse.GetResponse();

        }
    }
}
