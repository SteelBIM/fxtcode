using FxtDataAcquisition.Common;
using apiCommon = FxtDataAcquisition.API.Common;
using FxtDataAcquisition.API.Contract.APIInterface;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using System.ServiceModel.Activation;
using CAS.Entity;
using log4net;
using FxtDataAcquisition.Domain.DTO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

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
        public Stream Entrance(string type, string sinfo, string info)//(string type, string name, string date, string code, string parameter)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ResultData result = new ResultData();

            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                result.returntext = "验证码错误";
                return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                result.returntext = "无权限访问此API";
                return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
            }
            else
            {
                object objClass = null;
                MethodInfo method = apiCommon.EntranceHelper.GetMethodInfo(type, functionname, out objClass);
                if (method == null || objClass == null)
                {
                    result.returntext = "未找到方法!";
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                        result.returntext = "方法参数有误!";
                        return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
                    }
                    objvalue = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        if (funinfoObj[parameterInfo.Name] == null)
                        {
                            result.returntext = "方法参数不存在!";
                            return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
                        }
                        if (funinfoObj[parameterInfo.Name].Type == JTokenType.Null)
                        {
                            objvalue[i] = null;
                            continue;
                        }
                        objvalue[i] = CommonUtility.valueType(parameterInfo.ParameterType, funinfoObj.Value<JValue>(parameterInfo.Name).Value);
                    }
                    var o = method.Invoke(objClass, objvalue);

                    if (o is ResultData)
                    {
                        result = o as ResultData;
                    }
                    else
                    {
                        var to = o as Task<ResultData>;
                        result = to.Result;
                    }
                    log.Info("data:" + result.data);
                    log.Info("returntext:" + result.returntext);
                    log.Info("returntype:" + result.returntype);

                    sw.Stop();
                    TimeSpan ts2 = sw.Elapsed;
                    log.Info("functionname：" + functionname + "，操作总共花费" + ts2.TotalMilliseconds + "ms.");

                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException exe)
                {
                    log.Error("实体验证失败：" + exe.Message);
                    result.returntext = "实体验证失败!";
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
                }
                catch (Exception exe)
                {
                    string errorMessage = string.Format("type:{0},\r\n sinfo:{1}, \r\n info:{2}  \r\n error:{3}",
                        type == null ? "null" : type, sinfo == null ? "null" : sinfo, info == null ? "null" : info, exe.Message == null ? "null" : exe.Message
                        );
                    //log.Error(errorMessage, exe);
                    log.Error(exe);
                    result.returntext = "系统异常!";
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
                }
            }
        }

        public Stream UpLoadFile(System.IO.Stream stream, string type, string sinfo, string info)
        {
            log.Info("sinfo:" + sinfo);
            log.Info("info:" + info);
            ResultData result = new ResultData();

            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                result.returntext = "验证码错误!";
                return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                result.returntext = "无权限访问此API";
                return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
            }
            else
            {
                object objClass = null;
                MethodInfo method = apiCommon.EntranceHelper.GetMethodInfo(type, functionname, out objClass);
                if (method == null || objClass == null)
                {
                    result.returntext = "未找到对象！";
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                        result.returntext = "未找到对象！";
                        return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                            result.returntext = "未找到对象！";
                            return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));

                }
                catch (Exception exe)
                {
                    string errorMessage = string.Format("type:{0},\r\n sinfo:{1}, \r\n info:{2}  \r\n error:{3}",
                        type == null ? "null" : type, sinfo == null ? "null" : sinfo, info == null ? "null" : info, exe.Message == null ? "null" : exe.Message
                        );
                    log.Error(exe);
                    result.returntext = "系统异常！";
                    return new MemoryStream(Encoding.UTF8.GetBytes(result.ToJSONjss()));
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
