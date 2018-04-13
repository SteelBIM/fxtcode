using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using CourseActivate.Web.API.Models;
using CourseActivate.Core.Utility;
using System.Web.Http;
using Kingsun.Common;
using CourseActivate.Activate.BLL;
using CourseActivate.Activate.Constract.Model;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using CourseActivate.Framework.DAL;

namespace CourseActivate.Web.API.Controllers
{
    public class ActivateController : ApiController
    {
        /// <summary>
        /// 获取课程信息通过激活码
        /// </summary>
        /// <param name="Info">
        /// Info格式{ "UID": "123123123","UName": "3434324342","CourseID": 12321,"Code": "123123123123", "DeviceType": 0,"DeviceCode": "123121232","RTime":"1316545"}
        /// UID用户编号，UName用户名称，CourseID课本编号，Code激活码，DeviceType设备类型，DeviceCode设备编码
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public static KingResponse GetCourseByActivateCode(string Info)
        {
            try
            {
                CourseActivatKeyInfo parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(Info);
                if (parmModel != null)
                {
                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    parmModel.Code = parmModel.Code.ToUpper();
                    KingResponse res = bll.ActivateCourse(parmModel.Code, parmModel.DeviceType, parmModel.DeviceCode, parmModel.isios);
                    if (!res.Success) return res;
                    string[] resdata = res.Data.ToString().Split('|');
                    string resStr = "";
                    string BookID = "";
                    if (resdata.Length > 1)
                    {
                        BookID = resdata[1].Replace("\"", "");
                        resStr = resdata[0];
                    }
                    else
                    {
                        return KingResponse.GetErrorResponse("激活信息返回有误");
                    }
                    #endregion

                    #region 获取资源
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Encoding = Encoding.UTF8;
                    string returnText = wc.DownloadString(ConfigItemHelper.WebHost + "/Upload/BookResource/" + BookID + ".json");
                    #endregion

                    if (string.IsNullOrEmpty(returnText))
                    {
                        return KingResponse.GetErrorResponse("没有获取到资源信息");
                    }
                    returnText = returnText.Substring(0, returnText.LastIndexOf("}"));
                    returnText = returnText + resStr + "}";
                    returnText = returnText.Replace("@", ConfigItemHelper.WebHost);
                    return KingResponse.GetResponse(returnText);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }
            }
            catch (System.Net.WebException ex)
            {
                return KingResponse.GetErrorResponse(115, ErrorMsgCode.ErrorDic[115]);
            }
            catch (Exception ex)
            {

                return KingResponse.GetErrorResponse("请求接口异常。" + ex.Message);
            }
        }

        /// <summary>
        /// 通过激活码获取课程信息
        /// </summary>
        /// <param name="Info">Info只需要一个Code参数</param>
        /// <returns></returns>
        public static KingResponse GetCourseInfoByActivateCode(string Info)
        {
            try
            {
                CourseActivatKeyInfo parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(Info);
                if (parmModel != null)
                {
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    parmModel.Code = parmModel.Code.ToUpper();
                    KingResponse res = bll.GetBookID(parmModel.Code);
                    if (!res.Success) return res;

                    string[] resdata = res.Data.ToString().Split('|');                    
                    string BookID = "";
                    string Remark = "";
                    if (resdata.Length > 1)
                    {
                        BookID = resdata[0];
                        Remark = resdata[1];
                    }
                    else
                    {
                        return KingResponse.GetErrorResponse("获取课程信息返回有误");
                    }

                    #region 获取资源
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Encoding = Encoding.UTF8;
                   // TestLog4Net.LogHelper.Info("资源路径:" + ConfigItemHelper.WebHost + "/Upload/BookResource/" + res.Data.ToString() + ".json");
                    string returnText = wc.DownloadString(ConfigItemHelper.WebHost + "/Upload/BookResource/" + BookID + ".json");

                    if (string.IsNullOrEmpty(returnText))
                    {
                        return KingResponse.GetErrorResponse("没有获取到资源信息");
                    }
                    returnText = returnText.Substring(0, returnText.LastIndexOf("}"));
                    returnText = returnText + Remark + "}";
                    returnText = returnText.Replace("@", ConfigItemHelper.WebHost);
                    return KingResponse.GetResponse(returnText);
                    #endregion
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }
            }
            catch (System.Net.WebException ex)
            {
                return KingResponse.GetErrorResponse(115, ErrorMsgCode.ErrorDic[115]);
            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("请求接口异常。" + ex.Message);
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public static KingResponse SaveErrorLog(string Info)
        {
            try
            {
                UserIPInfo parmModel = JsonHelper.FromJsonIgnoreNull<UserIPInfo>(Info);
                if (parmModel != null)
                {
                    TestLog4Net.LogHelper.Info("记录错误日志，用户ip:" + parmModel.IP + ",日志类型=" + parmModel.type);
                    return KingResponse.GetResponse("写入日志成功");
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }
            }
            catch (System.Net.WebException ex)
            {
                return KingResponse.GetErrorResponse(115, ErrorMsgCode.ErrorDic[115]);
            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("请求接口异常。" + ex.Message);
            }
        }


        /// <summary>
        /// 通过模块编号获取资源信息
        /// </summary>
        /// <param name="Info">
        ///  Info格式{ "ModularID": 123123123,"RTime":"1316545"}
        ///  ModularID 模块ID,RTime时间戳
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public static KingResponse GetResourceByModularID(string Info)
        {
            try
            {
                CourseActivatKeyInfo parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(Info);
                if (string.IsNullOrEmpty(parmModel.CourseID))
                {
                    return KingResponse.GetErrorResponse("CourseID不能为空");
                }
                if (parmModel.ModularID.HasValue)
                {
                    ResourceBLL bll = new ResourceBLL();
                    tb_res_resource resinfo = bll.GetResourceByModulerID(parmModel.ModularID.Value, int.Parse(parmModel.CourseID));

                    if (resinfo == null)
                    {
                        return KingResponse.GetErrorResponse("没有找到模块资源信息");
                    }
                    if (resinfo.Status != 1)
                    {
                        return KingResponse.GetErrorResponse("该模块资源未启用");
                    }
                    resinfo.ResUrl = resinfo.ResUrl.Replace("@", ConfigItemHelper.WebHost);
                    //var obj = new
                    //{
                    //    ModularID = parmModel.ModularID,
                    //    ModularName = resinfo.ModularName,
                    //    ResUrl = resinfo.ResUrl,
                    //    ResMD5 = resinfo.ResMD5,
                    //    ResVersion = resinfo.ResVersion,
                    //    ResKey = resinfo.ResKey,
                    //    isForce = resinfo.isForce
                    //};
                    return KingResponse.GetResponse(JsonHelper.ToJson(resinfo));
                }
                else
                {
                    return KingResponse.GetErrorResponse("ModularID不能为空");
                }
            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("请求接口异常。" + ex.Message);
            }
        }


        /// <summary>
        /// 通过AppID获取App最新版本信息
        /// </summary>
        /// <param name="Info">只需要AppID参数</param>
        /// <returns></returns>
        [HttpPost]
        public static KingResponse GetAppVersionInfo(string Info)
        {
            AppVersionModel model = JsonHelper.FromJsonIgnoreNull<AppVersionModel>(Info);

            if (model == null)
            {
                return KingResponse.GetErrorResponse("未获取到参数信息");
            }
            if (!model.AppID.HasValue)
            {
                return KingResponse.GetErrorResponse("请输入AppID");
            }
            //if (string.IsNullOrEmpty(model.Version))
            //{
            //    return KingResponse.GetErrorResponse("未获取到参数信息");
            //}

            //最新版本根据时间来定
            ResourceBLL bll = new ResourceBLL();
            tb_res_appversion version = bll.GetAppNewVersionByAppID(model.AppID.Value);
            if (version == null)
            {
                return KingResponse.GetErrorResponse("没有找到App相关版本信息");
            }
            version.Url = version.Url.Replace("@", ConfigItemHelper.WebHost);

            return KingResponse.GetResponse(JsonHelper.ToJson(version));
        }

        /// <summary>
        /// 激活测试
        /// </summary>
        /// <param name="activatecode"></param>
        /// <param name="devicetype"></param>
        /// <param name="devicecode"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse ApiActivateCourse(string activatecode, int devicetype, string devicecode, int? isios)
        {
            CourseActivatKeyInfo info = new CourseActivatKeyInfo();
            info.Code = activatecode;
            info.DeviceType = devicetype;
            info.DeviceCode = devicecode;
            info.isios = isios ?? 0;
            return GetCourseByActivateCode(JsonHelper.EncodeJson(info));
        }

        /// <summary>
        /// 测试获取模块资源信息
        /// </summary>
        /// <param name="modularid"></param>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public KingResponse ApiGetResourceByModularID(int modularid, int bookid)
        {
            var obj = new { ModularID = modularid, CourseID = bookid };
            return GetResourceByModularID(JsonHelper.EncodeJson(obj));
        }

        /// <summary>
        /// 通过激活码获取课程信息
        /// </summary>
        /// <param name="activatecode"></param>
        /// <returns></returns>
        public KingResponse ApiGetCourseInfoByActivateCode(string activatecode)
        {
            var obj = new { Code = activatecode };
            return GetCourseInfoByActivateCode(JsonHelper.EncodeJson(obj));
        }

        /// <summary>
        /// 通过AppID获取App最新版本信息
        /// </summary>
        /// <param name="model">只需要AppID参数</param>
        /// <returns></returns>
        public KingResponse ApiGetAppVersionInfo([FromBody]AppVersionModel model)
        {
            return GetAppVersionInfo(JsonHelper.EncodeJson(model));
        }

        [HttpPost]
        public KingResponse ApiTestAddRedis(int count)
        {
            new Activate.BLL.ActivateCourseBLL().AddResitTestData(count);
            return KingResponse.GetResponse("");
        }

        [HttpPost]
        public KingResponse ApiTestRemoveRedis()
        {
            // new Activate.BLL.ActivateCourseBLL().RemoveRedisList();
            new Activate.BLL.ActivateCourseBLL().SyncRedisToDB();
            return KingResponse.GetResponse("");
        }

    }

    public class TempParam
    {
        public string Key { get; set; }
        public string Info { get; set; }
        public string FunName { get; set; }
    }

}