using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CAS.Entity;
using CAS.Entity.CASEntity;

namespace CAS.Common
{
    public class WCFAPIHelper
    {
        /// <summary>
        /// 调用WCFAPI（不包含附件上传与下载）CASCommon.GetWCFAPIResult<T>,T为returnData.data的类型
        /// </summary>
        /// <typeparam name="T">returnData.data的类型</typeparam>
        /// <param name="apiUrl">apiUrl</param>
        /// <param name="posts">传入参数</param>
        /// <param name="list">返回data是否为list</param>
        /// <edit>
        /// ***2014-04-24 hody*******
        /// 创建
        /// </edit>
        /// <returns></returns>
        public static JSONHelper.ReturnData GetWCFAPIResult<T>(string apiUrl, string posts)
        {
            return GetWCFAPIResult<T>(apiUrl, posts, false);
        }

        /// <summary>
        /// 调用WCFAPI（不包含附件上传与下载）CASCommon.GetWCFAPIResult<T>,T为returnData.data的类型
        /// </summary>
        /// <typeparam name="T">returnData.data的类型</typeparam>
        /// <param name="apiUrl">apiUrl</param>
        /// <param name="posts">传入参数</param>
        /// <param name="list">返回data是否为list</param>
        /// <edit>
        /// ***2014-04-24 hody*******
        /// 创建
        /// </edit>
        /// <returns></returns>
        public static JSONHelper.ReturnData GetWCFAPIResult<T>(string apiUrl, string posts, bool list)
        {


            JSONHelper.ReturnData returnData = new JSONHelper.ReturnData();

            WCFJsonData jsondata = new WCFJsonData();
            string result = string.Empty;
            result = WebCommon.WcfApiMethodOfPost(apiUrl, posts);
            if (!string.IsNullOrEmpty(result))
            {
                jsondata = JSONHelper.JSONToObject<WCFJsonData>(result);
                if (list)
                {
                    returnData.data = JSONHelper.JSONStringToList<T>(jsondata.data);
                }
                else
                {
                    returnData.data = JSONHelper.JSONToObject<T>(jsondata.data);
                }

                returnData.debug = jsondata.debug;
                returnData.returntype = jsondata.returntype;
                returnData.returntext = jsondata.returntext;
            }
            else
            {
                returnData.data = "";
                returnData.returntext = "失败";
                returnData.returntype = (int)EnumHelper.Status.Failure;
            }
            return returnData;
        }


        /// <summary>
        /// 生成WCFAPI接口参数（不包含登录）
        /// </summary>
        /// <param name="GJBWX_SInfoOfConfigKey">sinfo</param>
        /// <param name="objUinfo">用户信息：暂时未使用</param>
        /// <param name="objAppInfo">平台信息：暂时未使用</param>
        /// <param name="funinfo">功能参数信息</param>
        /// <edit>
        /// ***2014-04-24 hody*******
        /// 创建
        /// </edit>
        /// <returns></returns>
        public static string GenerateWCFAPIParameters(WCFSInfo objSinfo, WCFUInfo objUinfo, WCFAPPInfo objAppInfo, Object objFunInfo)
        {
            string appid = objSinfo.appid.ToString();
            string apppwd = objSinfo.apppwd,
                   signname = objSinfo.signname,
                   time = objSinfo.time,
                   appkey = objSinfo.appkey;

            string[] pwdArray = { appid, apppwd, signname, time, objSinfo.functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            var args = new
            {
                sinfo = new
                {
                    appid = appid,
                    apppwd = apppwd,
                    signname = signname,
                    time = time,
                    code = code,
                    functionname = objSinfo.functionname
                }.ToJson(),
                info = new
                {
                    appinfo = new
                    {
                        splatype = objAppInfo.splatype,
                        stype = objAppInfo.stype,
                        version = objAppInfo.version,
                        vcode = objAppInfo.vcode,
                        systypecode = objAppInfo.systypecode,
                        channel = objAppInfo.channel
                    },
                    uinfo = new { username = objUinfo.username, token = objUinfo.token },
                    funinfo = objFunInfo
                }.ToJson()
            };
            return args.ToJson();
        }


        /// <summary>
        /// 生成WCFAPI登录接口参数
        /// </summary>
        /// <param name="GJBWX_SInfoOfConfigKey">sinfo</param>
        /// <param name="objUinfo">用户信息</param>
        /// <param name="objAppInfo">平台信息</param>
        /// <edit>
        /// ***2014-04-28 hody*******
        /// 创建
        /// </edit>
        /// <returns></returns>
        public static string GenerateWCFAPILoginParameters(WCFSInfo objSinfo, WCFUInfo objUinfo, WCFAPPInfo objAppInfo)
        {
            objUinfo.password = EncryptHelper.GetMd5(objUinfo.password, ConstCommon.WcfPassWordMd5Key);
            string code = EncryptHelper.GetMd5(objSinfo.time, ConstCommon.WcfLoginMd5Key);

            var args = new
            {
                sinfo = new
                {
                    time = objSinfo.time,
                    code = code
                }.ToJson(),
                info = new
                {
                    appinfo = new
                    {
                        splatype = objAppInfo.splatype,
                        stype = objAppInfo.stype,
                        version = objAppInfo.version,
                        vcode = objAppInfo.vcode,
                        systypecode = objAppInfo.systypecode,
                        channel = objAppInfo.channel
                    },
                    uinfo = new { username = objUinfo.username, password = objUinfo.password },
                    funinfo = new { weburl = HttpContext.Current.Request.Url.Authority }
                }.ToJson()
            };
            return args.ToJson();
        }
    }
}
