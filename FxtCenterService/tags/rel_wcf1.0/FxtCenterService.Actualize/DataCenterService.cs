using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtCenterService.Contract;
using System.ServiceModel.Activation;
using CAS.Entity;
using CAS.Common;
using System.Xml;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using FxtCenterService.Actualize;

namespace FxtCenterService.Actualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataCenterService:IDataCenterService
    {

        public WCFJsonData Entrance(string sinfo, string info)
        {
            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                JObject objSinfo = JObject.Parse(sinfo);
                JObject objinfo = JObject.Parse(info);
                JObject funinfo = objinfo["funinfo"] as JObject;
                JObject appinfo = objinfo["appinfo"] as JObject;
                JObject uinfo = objinfo["uinfo"] as JObject;
                string funtionname = objSinfo.Value<string>("functionname");
                //SearchBase search = FxtGjbApiCommon.InitSearBase(funinfo);//初始化SearchBase
                string returntext = string.Empty;
                int returntype = 0;
                UserCheck company = new UserCheck();

                #region 安全验证
                if (!string.IsNullOrEmpty(objSinfo.Value<string>("appid")) || !string.IsNullOrEmpty(objSinfo.Value<string>("apppwd")))
                {
                    string securityJson = WebCommon.ApiSecurityStringOfJson(objSinfo, funtionname, objinfo);
                    LogHelper.Info(securityJson);
                    // FxtGjbApiCommon.WcfApiMethodOfPost(Constant.WCFUserCenterService, securityJson);
                    company = WebCommon.FxtUserCenterService_GetCompanyBySignName(securityJson, out returntext, out returntype);
                    LogHelper.Info(company.ToJson());
                    if (company != null && returntype == 1)
                    {
                        company.username = uinfo.Value<string>("username");
                        if (!string.IsNullOrEmpty(funtionname))
                        {
                            MethodInfo methInfo = DataCenterCommon.GetMethodInfo(funtionname);
                            ParameterInfo[] parameterInfo = methInfo.GetParameters();
                            object[] objParamet = { funinfo,company };
                            jsonData.data = methInfo.Invoke(null, objParamet) as string;
                        }
                        else
                        {
                            jsonData.returntype = returntype;
                            jsonData.returntext = returntext;
                        }
                    }
                    else
                    {
                        jsonData.returntype = returntype;
                        jsonData.returntext = returntext;
                    }
                }
                else
                {
#if DEBUG
                    jsonData.returntype = returntype;
                    jsonData.returntext = returntext;
#else
                      jsonData.returntype = returntype;
                     jsonData.returntext = returntext;
#endif
                }
                #endregion
                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonData.data = "";
                jsonData.returntext = "errorjsonData";
                jsonData.returntype = -1;
                return jsonData;
            }
        }

        ///// <summary>
        ///// 读取SQL语句，注意文件名和传入的参数大小写要匹配
        ///// SQL文件必须修改为“嵌入的资源”
        ///// 如果不用嵌入也可以使用文件方式读取，好处是应用程序不会重启，但明文存放可能引起容易被篡改等安全问题
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        public static string xmlApiConfig(string xmlPath)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = xmlPath + ".xml";
            string result = "";
            try
            {
                Stream stream = _assembly.GetManifestResourceStream(resourceName);
                StreamReader myread = new StreamReader(stream);
                result = myread.ReadToEnd();
                myread.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //转为小写，避免sql与前台json大小写不一致
            return result;
        }
    }


}
