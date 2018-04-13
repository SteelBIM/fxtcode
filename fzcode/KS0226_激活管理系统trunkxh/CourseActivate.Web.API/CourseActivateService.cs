using CourseActivate.Core.Utility;
using CourseActivate.Web.API.Controllers;
using CourseActivate.Web.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;

namespace CourseActivate.Web.API
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CourseActivateService : ICourseActivateService
    {
        private static object o = new object();
        public KingResponse Entrance(string Key, string Info, string FunName)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(Info))
            {
                return KingResponse.GetErrorResponse("参数Info为空!");
            }
            else if (string.IsNullOrEmpty(FunName))
            {
                return KingResponse.GetErrorResponse("参数FunName为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(Key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(Info, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    if (ts.TotalSeconds > int.Parse(ConfigurationManager.AppSettings["RTimeValidSeconds"])) return KingResponse.GetErrorResponse(401, "请求接口失效");
                    #endregion

                    #region 业务数据
                    object[] objinfo = new object[] { dataStr };
                    Type objectCon = typeof(ActivateController);
                    MethodInfo methInfo = objectCon.GetMethod(FunName);
                    KingResponse returnvlue = (KingResponse)methInfo.Invoke(null, objinfo);
                    if (!returnvlue.Success) return returnvlue;
                    #endregion

                    #region 加密返回参数
                    //取出data中的key（未加密）,用来加密我自动生成的key
                    //再用我生成的key去加密返回的data
                    //客户端传过来的公钥
                    parmModel.PKey = parmModel.PKey.Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");
                    //服务端生成8位key
                    string keyTmp =Guid.NewGuid().ToString().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(returnvlue.Data);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion
                    KingResponse res = KingResponse.GetResponse(JsonHelper.EncodeJson(result));
                    return res;
                    // return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("接口请求异常：" + ex.Message);
            }
        }
    }
}