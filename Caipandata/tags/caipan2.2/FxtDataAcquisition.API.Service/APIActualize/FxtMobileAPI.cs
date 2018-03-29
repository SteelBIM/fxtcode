using FxtDataAcquisition.Common;
using apiCommon = FxtDataAcquisition.API.Common;
using FxtDataAcquisition.API.Contract.APIInterface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CAS.Common.MVC4;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using Newtonsoft.Json;
using System.IO;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using CAS.Entity;
using System.Data;
using log4net;
using FxtDataAcquisition.Domain.DTO;
using AutoMapper;
using FxtDataAcquisition.Domain.Models;
using System.Threading.Tasks;

namespace FxtDataAcquisition.API.Service.APIActualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FxtMobileAPI : IFxtMobileAPI
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(FxtMobileAPI));
        #region 公共入口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sinfo">加密验证参数{appid:"",apppwd:"",signname:"",time:"",code:""}</param>
        /// <param name="functionname">功能名</param>
        /// <param name="functionparameter">功能参数</param>
        /// <returns></returns>
        public ResultData Entrance(string type, string sinfo, string info)//(string type, string name, string date, string code, string parameter)
        {
            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                return new ResultData { returntype = 0, returntext = "验证码错误" };
            }
            JObject sinfoObj = JObject.Parse(sinfo);
            string appid = sinfoObj.Value<string>("appid");
            string apppwd = sinfoObj.Value<string>("apppwd");
            string signname = sinfoObj.Value<string>("signname");
            string time = sinfoObj.Value<string>("time");
            string code = sinfoObj.Value<string>("code");
            string functionname = sinfoObj.Value<string>("functionname");
            string appUrl = "";
            JObject infoObj = JObject.Parse(info);
            JObject appinfoObj = infoObj["appinfo"] as JObject;
            JObject uinfoObj = infoObj["uinfo"] as JObject;
            string username = uinfoObj.Value<string>("username");
            JObject funinfoObj = new JObject();
            if (infoObj["funinfo"] != null)
            {
                funinfoObj = infoObj["funinfo"] as JObject;
            }
            log.Info(sinfo);
            log.Info(info);

            UserCheck uc = null;
            int verifyResult = ApiCheckManager.ApiAuthorizationVerify(sinfoObj, infoObj, functionname, out uc);
            if (verifyResult != 1)
            {
                return new ResultData { returntype = 0, returntext = "无权限访问此API" };
            }
            else
            {
                object objClass = null;
                MethodInfo method = apiCommon.EntranceHelper.GetMethodInfo(type, functionname, out objClass);
                if (method == null || objClass == null)
                {
                    return new ResultData { returntype = 0, returntext = "未找到对象!" };
                }
                try
                {
                    Type objClassType = objClass.GetType();
                    PropertyInfo UserName = objClassType.GetProperty("UserName");
                    PropertyInfo SignName = objClassType.GetProperty("SignName");
                    PropertyInfo FxtCompanyId = objClassType.GetProperty("FxtCompanyId");
                    UserName.SetValue(objClass, username, null);
                    SignName.SetValue(objClass, signname, null);
                    FxtCompanyId.SetValue(objClass, uc.companyid, null);
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    object[] objvalue = null;
                    if (funinfoObj.Count != parameterInfos.Length)
                    {
                        return new ResultData { returntype = 0, returntext = "未找到对象!" };
                    }
                    objvalue = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        if (funinfoObj[parameterInfo.Name] == null)
                        {
                            return new ResultData { returntype = 0, returntext = "未找到对象!" };
                        }
                        if (funinfoObj[parameterInfo.Name].Type == JTokenType.Null)
                        {
                            objvalue[i] = null;
                            continue;
                        }
                        objvalue[i] = CommonUtility.valueType(parameterInfo.ParameterType, funinfoObj.Value<JValue>(parameterInfo.Name).Value);
                    }
                    var o = method.Invoke(objClass, objvalue);
                    ResultData obj;
                    if (o is ResultData)
                    {
                        obj = o as ResultData;
                    }
                    else
                    {
                        var to = o as Task<ResultData>;
                        obj = to.Result;
                    }
                    log.Info(obj.data);
                    log.Info(obj.returntext);
                    log.Info(obj.returntype);
                    return obj;

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException exe)
                {
                    log.Error("实体验证失败：" + exe.Message);
                    return new ResultData { returntype = 0, returntext = "系统异常", debug = exe.Message };
                }
                catch (Exception exe)
                {
                    string errorMessage = string.Format("type:{0},\r\n sinfo:{1}, \r\n info:{2}  \r\n error:{3}",
                        type == null ? "null" : type, sinfo == null ? "null" : sinfo, info == null ? "null" : info, exe.Message == null ? "null" : exe.Message
                        );
                    //log.Error(errorMessage, exe);
                    log.Error(exe);
                    return new ResultData { returntype = 0, returntext = "系统异常", debug = exe.Message };
                }
            }
        }

        public ResultData UpLoadFile(System.IO.Stream stream, string type, string sinfo, string info)
        {
            log.Info("sinfo:" + sinfo);
            log.Info("info:" + info);
            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                return new ResultData { returntype = 0, returntext = "验证码错误" };
            }
            JObject sinfoObj = JObject.Parse(sinfo);
            string appid = sinfoObj.Value<string>("appid");
            string apppwd = sinfoObj.Value<string>("apppwd");
            string signname = sinfoObj.Value<string>("signname");
            string time = sinfoObj.Value<string>("time");
            string code = sinfoObj.Value<string>("code");
            string functionname = sinfoObj.Value<string>("functionname");
            string appUrl = "";
            JObject infoObj = JObject.Parse(info);
            JObject appinfoObj = infoObj["appinfo"] as JObject;
            JObject uinfoObj = infoObj["uinfo"] as JObject;
            string username = uinfoObj.Value<string>("username");
            JObject funinfoObj = new JObject();
            if (infoObj["funinfo"] != null)
            {
                funinfoObj = infoObj["funinfo"] as JObject;
            }
            UserCheck uc = null;
            int verifyResult = ApiCheckManager.ApiAuthorizationVerify(sinfoObj, infoObj, functionname, out uc);
            if (verifyResult != 1)
            {
                return new ResultData { returntype = 0, returntext = "无权限访问此API" };
            }
            else
            {
                object objClass = null;
                MethodInfo method = apiCommon.EntranceHelper.GetMethodInfo(type, functionname, out objClass);
                if (method == null || objClass == null)
                {
                    return new ResultData { returntype = 0, returntext = "未找到对象!" };
                }
                try
                {
                    Type objClassType = objClass.GetType();
                    PropertyInfo UserName = objClassType.GetProperty("UserName");
                    PropertyInfo SignName = objClassType.GetProperty("SignName");
                    PropertyInfo FxtCompanyId = objClassType.GetProperty("FxtCompanyId");
                    UserName.SetValue(objClass, username, null);
                    SignName.SetValue(objClass, signname, null);
                    FxtCompanyId.SetValue(objClass, uc.companyid, null);
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    object[] objvalue = null;
                    if (funinfoObj.Count + 1 != parameterInfos.Length)
                    {
                        return new ResultData { returntype = 0, returntext = "未找到对象!" };
                    }
                    objvalue = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        if (i == 0)
                        {
                            objvalue[i] = stream;
                            continue;
                        }
                        ParameterInfo parameterInfo = parameterInfos[i];
                        if (funinfoObj[parameterInfo.Name] == null)
                        {
                            return new ResultData { returntype = 0, returntext = "未找到对象!" };
                        }
                        if (funinfoObj[parameterInfo.Name].Type == JTokenType.Null)
                        {
                            objvalue[i] = null;
                            continue;
                        }
                        objvalue[i] = CommonUtility.valueType(parameterInfo.ParameterType, funinfoObj.Value<JValue>(parameterInfo.Name).Value);
                    }
                    var o = method.Invoke(objClass, objvalue);
                    ResultData obj;
                    if (o is ResultData)
                    {
                        obj = o as ResultData;
                    }
                    else
                    {
                        var to = o as Task<ResultData>;
                        obj = to.Result;
                    }
                    return obj;

                }
                catch (Exception exe)
                {
                    string errorMessage = string.Format("type:{0},\r\n sinfo:{1}, \r\n info:{2}  \r\n error:{3}",
                        type == null ? "null" : type, sinfo == null ? "null" : sinfo, info == null ? "null" : info, exe.Message == null ? "null" : exe.Message
                        );
                    log.Error(exe);
                    return new ResultData { returntype = 0, returntext = "系统异常", debug = exe.Message };
                }
            }
        }

        //public ResultData GetFileSeries_bak(string filename)
        //{
        //    long length = 0;
        //    if (!string.IsNullOrEmpty(filename))
        //    {
        //        string folder = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        string path = Path.Combine(folder, filename);
        //        if (File.Exists(path))
        //        {
        //            FileInfo file = new FileInfo(path);
        //            length = file.Length;
        //        }
        //    }
        //    return new ResultData { returntype = 1, data = length.ToString(), returntext = "" };
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="stream">文件字节流</param>
        ///// <param name="filename">文件名</param>
        ///// <param name="npos">此文件已经传输的大小(基本等于服务器已保存文件的大小)</param>
        ///// <returns></returns>
        //public ResultData UpLoadFileSeries_bak(System.IO.Stream stream, string filename, string npos)
        //{

        //    if (!string.IsNullOrEmpty(filename))
        //    {
        //        string folder = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        string path = Path.Combine(folder, filename);
        //        //保存文件
        //        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        //        { 
        //            //偏移指针
        //            fs.Seek(Convert.ToInt64(npos), SeekOrigin.Begin);
        //            long ByteLength = WebOperationContext.Current.IncomingRequest.ContentLength;
        //            byte[] fileContent = new byte[ByteLength];
        //            stream.Read(fileContent, 0, fileContent.Length);
        //            fs.Write(fileContent, 0, fileContent.Length);
        //            fs.Flush();
        //        }
        //    }
        //    return new ResultData {  data = "", returntext = "", returntype = 1 };
        //}

        #endregion

    }
}
