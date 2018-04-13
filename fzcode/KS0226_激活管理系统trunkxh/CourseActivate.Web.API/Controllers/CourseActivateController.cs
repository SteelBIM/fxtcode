using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using CourseActivate.Web.API.Models;
using CourseActivate.Core.Utility;
using System.Web.Http;
using Kingsun.Common;
using CourseActivate.Activate.BLL;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Framework.BLL;
using Newtonsoft.Json.Linq;

namespace CourseActivate.Web.API.Controllers
{
    public class CourseActivateController : ApiController
    {
        /// <summary>
        /// 课程加密解密
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse VerifyUserCourseActivateKey([FromBody]UserModel userModel)
        {
            //UserModel userModel = JsonHelper.DecodeJson<UserModel>(request.Data);
            if (userModel == null)
            {
                return KingResponse.GetErrorResponse("参数为空!");
            }
            //参数
            string key = userModel.Key;
            string data = userModel.Info;
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseResult result = new CourseResult();
            ParmaData parmModel = new ParmaData();
            try
            {
                string privatKey = ConfigItemHelper.PrivateKey;

                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");

                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);

                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<ParmaData>(dataStr);

                if (parmModel != null)
                {
                    #region 数据处理
                    //取出data中的key（未加密）,用来加密我自动生成的key
                    //再用我生成的key去加密返回的data

                    //客户端传过来的公钥
                    parmModel.PKey = parmModel.PKey.Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");
                    //服务端生成8位key
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");

                    CoursePart model = new CoursePart();
                    model.UpdateMD5 = "123";
                    model.UpdateURL = "123456";
                    model.Version = "1.1";
                    model.TryUpdate = 0;
                    model.PchKey = "1";

                    string strData = JsonHelper.ToJson(model);

                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }
                    #endregion
            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("获取课程包下载地址失败" + ex.Message);
            }
        }

        /// <summary>
        /// 课程激活验证
        /// </summary>
        /// <returns></returns>
        public KingResponse VerifyUserCourseActivate([FromBody]courseActivatemodel funinfo)
        {
            try
            {
                string bookid = "";
                if (funinfo.bookid.HasValue)
                {
                    bookid = funinfo.bookid.Value.ToString();
                }
                if (!funinfo.devicetype.HasValue)
                {
                    return new KingResponse
                    {
                        Success = false,
                        ErrorMsg = "设备类型不能为空",
                        Data = ""
                    };
                }
                if (string.IsNullOrEmpty(funinfo.devicecode))
                {
                    return KingResponse.GetErrorResponse("设备号为空");
                }
                ActivateCourseBLL bll = new ActivateCourseBLL();
                KingResponse res = bll.ActivateCourse(funinfo.userid, funinfo.username, bookid, funinfo.activatecode, funinfo.devicetype.Value, funinfo.devicecode);
                return res;
            }
            catch (Exception ex)
            {
                return new KingResponse
                {
                    Success = false,
                    ErrorMsg = "接口出错." + ex.Message,
                    Data = ""
                };
            }
        }


        /// <summary>
        /// 课程加密解密及激活验证
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse VerifyCourseActivateKey([FromBody]courseActivatemodelkey requestModel)
        {
            if (requestModel == null) return KingResponse.GetErrorResponse("参数为空!");
            //参数
            string key = requestModel.Key;
            string data = requestModel.Info;
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                Manage manage = new Manage();
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    if (ts.Seconds > 15) return KingResponse.GetErrorResponse(401, "请求接口失效");
                    #endregion

                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    KingResponse res = bll.ActivateCourse(parmModel.UID, parmModel.UName, parmModel.CourseID, parmModel.Code, parmModel.DeviceType, parmModel.DeviceCode);
                    if (!res.Success) return res;
                    #endregion
                   
                    #region 获取资源
                    SourceInfo model = new SourceInfo();
                    string time = bll.GetActivateEndTime(parmModel.UID, parmModel.CourseID);
                    model.ActivateTime = time;
                    bll.UpdateActivateStauts(parmModel.Code);
                    model.Months = ConfigurationManager.AppSettings["Months"].ToString();
                    if (parmModel.Channel == 0)
                    {
                        //    //客户端资源                        
                        //    model.SourceMD5 = "123";
                        //    model.SourceURL = "123456";
                        //    model.SourceVersion = "1.1";
                        //    model.SourceKey = "1";
                    }
                    else
                    {
                        //同步学资源
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                        
                        manage.SynInsert<TB_UserMember>(new TB_UserMember
                        {
                            ID = Guid.NewGuid(),
                            CourseID = parmModel.CourseID,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(12),
                            UserID = parmModel.UID,
                            Months = 12,
                            CreateTime = DateTime.Now,
                            Status = 0,
                            TbOrderID = Guid.NewGuid()
                            //TbOrderID = TbOrderID
                        });

                    }
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
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(model);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("课程加密解密及激活验证失败" + ex.Message);
            }
        }

        /// <summary>
        /// 课程支付激活码接口，匹配一个在线未使用激活码
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse PayVerifyCourseActivateKey([FromBody]courseActivatemodelkey requestModel)
        {
            if (requestModel == null) return KingResponse.GetErrorResponse("参数为空!");
            //参数
            string key = requestModel.Key;
            string data = requestModel.Info;
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    if (ts.Seconds > 15) return KingResponse.GetErrorResponse(401, "请求接口失效");
                    #endregion

                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    //查找激活码
                    string activatecode = bll.MatchActivateCode();
                    if (!string.IsNullOrEmpty(activatecode))
                    {
                        //bll.UpdateActivateStauts(activatecode);
                        KingResponse res = bll.ActivateCourse(parmModel.UID, parmModel.UName, parmModel.CourseID, activatecode, parmModel.DeviceType, parmModel.DeviceCode);
                        if (!res.Success) return res;
                    }
                    else
                    {
                        return KingResponse.GetErrorResponse("未匹配激活码");
                    }

                    #endregion

                    #region 获取资源
                    SourceInfo model = new SourceInfo();
                    string time = bll.GetActivateEndTime(parmModel.UID, parmModel.CourseID);
                    model.ActivateTime = time;
                    model.Months = ConfigurationManager.AppSettings["Months"].ToString();
                    if (parmModel.Channel == 0)
                    {
                        //客户端资源                        
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
                    else
                    {
                        //同步学资源
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
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
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(model);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("课程加密解密及激活验证失败" + ex.Message);
            }
        }


        /// <summary>
        /// 课程查找激活码接口，通过bookid+userid查找对应激活码
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse UseVerifyCourseActivateKey([FromBody]courseActivatemodelkey requestModel)
        {
            if (requestModel == null) return KingResponse.GetErrorResponse("参数为空!");
            //参数
            string key = requestModel.Key;
            string data = requestModel.Info;
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    if (ts.Seconds > 15) return KingResponse.GetErrorResponse(401, "请求接口失效");
                    #endregion

                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    //匹配激活码
                    List<tb_batchactivateuse> use = bll.SearchBatchActivateUse(parmModel.UID, parmModel.CourseID);
                    if (use != null && use.Count > 0)
                    {
                        KingResponse res = bll.ActivateCourse(parmModel.UID, parmModel.UName, parmModel.CourseID, use[0].activatecode, parmModel.DeviceType, parmModel.DeviceCode);
                        if (!res.Success) return res;
                    }
                    else
                    {
                       return  KingResponse.GetErrorResponse(402, "用户未购买过此课程");
                    }

                    #endregion

                    #region 获取资源
                    SourceInfo model = new SourceInfo();
                    if (parmModel.Channel == 0)
                    {
                        //客户端资源                        
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
                    else
                    {
                        //同步学资源
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
                    string time = bll.GetActivateEndTime(parmModel.UID, parmModel.CourseID);
                    model.ActivateTime = time;
                    model.Months = ConfigurationManager.AppSettings["Months"].ToString();
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
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(model);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("课程加密解密及激活验证失败" + ex.Message);
            }
        }



        [HttpGet]
        public KingResponse test()
        {
            //参数
            string key =
                "pIVRJSuX4beaCZ62LJXLaNC36UcxZx/hedk1EF9HxOk2oXGo2gO6ZllHfJ78hWL3icc0iXmjh2llRWe9iEa+fWbArfmunsuvz/lxyFMXFUZMZ4CDx5la4z50yzwqIgiXd0Qb8TUlLJjxhHSchU9ZFdESy4wbRaZ16tNiR4WTeE4=";
            string data =
                "g4k5Gx4JQl35dstn1eknbb5F6kHR0zUy4jT+HuLGk9ug6aSYjhAN0SQHxtDWtGNmVdyQkvG9Sdz0SMbNDYOXngeDp0zUPSALFMUL88UvHubERtrgLeCtZrie+dbFZnrnYcFDTsGwiq/aiAKznnOShmVCftZI+HI3wMyotcqtxt5c/Qoe+prmRnW2DHDY0zQHdMIHBxOp1obpf4riqULyy4UqtiVX491iMKqM+3mJdgvojTg06LCsn9aUOb6CFGVQ3eCa+pa1HrOsPWXmsjB2JRTVtOPhQpHz+A/JZKN2M9u0ZGsPUGFQAZEMYIl5HI4foKDCMI7BilJJWzLnm7Txg9QMKrVfhEfVYbWhVW48Vx3dRjNJm4uzlCwhzVhz71D8QMRSYXjnVlDs8SfFYz2MlN1vTeko8cgS4HL1J69YiWMob5Gdyl9tyE3fmilGUBjhLwjf6qWEA6H5mhY//Bn9O4/AJCOcY0WrQ1EGe+0NXTgMWvu49+xZCeZFnW6gFoxDamZjoty/KwBmXB2orS2KFIw9G2wfDsNBODfAorHH0IanAkyOR6hjhYjuuDK3c9ajCUwtBLLeWuQsvJ5HBmByMFmZsj8s7ylaB6IkluttGyeJuh0fFJ/98Pf2BtS7+LnArU0gSnrTh2OTWZU68DboBRtIt44XgVgK/Y3Ew2KH6jfhDNz1Snv/0US7bTSYfvUOti8oRKrFvA037Hqq4Yp4EkJObbeHIqV+h7fZA2yD+UA=";
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证

                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    if (ts.Seconds > 15) return KingResponse.GetErrorResponse(401, "请求接口失效");
                    #endregion

                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    //匹配激活码
                    List<tb_batchactivateuse> use = bll.SearchBatchActivateUse(parmModel.UID, parmModel.CourseID);
                    if (use != null && use.Count > 0)
                    {
                        KingResponse res = bll.ActivateCourse(parmModel.UID, parmModel.UName, parmModel.CourseID, use[0].activatecode, parmModel.DeviceType, parmModel.DeviceCode);
                        if (!res.Success) return res;
                    }
                    else
                    {
                        KingResponse.GetErrorResponse(402, "用户未购买过此课程");
                    }

                    #endregion

                    #region 获取资源
                    SourceInfo model = new SourceInfo();
                    if (parmModel.Channel == 0)
                    {
                        //客户端资源                        
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
                    else
                    {
                        //同步学资源
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";
                    }
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
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(model);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("课程加密解密及激活验证失败" + ex.Message);
            }
        }

        [HttpPost]
        public KingResponse test1()
        {
            //参数
            string key =
                "pIVRJSuX4beaCZ62LJXLaNC36UcxZx/hedk1EF9HxOk2oXGo2gO6ZllHfJ78hWL3icc0iXmjh2llRWe9iEa+fWbArfmunsuvz/lxyFMXFUZMZ4CDx5la4z50yzwqIgiXd0Qb8TUlLJjxhHSchU9ZFdESy4wbRaZ16tNiR4WTeE4=";
            string data =
                "g4k5Gx4JQl35dstn1eknbb5F6kHR0zUy4jT+HuLGk9ug6aSYjhAN0SQHxtDWtGNmVdyQkvG9Sdz0SMbNDYOXngeDp0zUPSALFMUL88UvHubERtrgLeCtZrie+dbFZnrnYcFDTsGwiq/aiAKznnOShmVCftZI+HI3wMyotcqtxt5c/Qoe+prmRnW2DHDY0zQHdMIHBxOp1obpf4riqULyy4UqtiVX491iMKqM+3mJdgvojTg06LCsn9aUOb6CFGVQ3eCa+pa1HrOsPWXmsjB2JRTVtOPhQpHz+A/JZKN2M9u0ZGsPUGFQAZEMYIl5HI4foKDCMI7BilJJWzLnm7Txg9QMKrVfhEfVYbWhVW48Vx3dRjNJm4uzlCwhzVhz71D8QMRSYXjnVlDs8SfFYz2MlN1vTeko8cgS4HL1J69YiWMob5Gdyl9tyE3fmilGUBjhLwjf6qWEA6H5mhY//Bn9O4/AJCOcY0WrQ1EGe+0NXTgMWvu49+xZCeZFnW6gFoxDamZjoty/KwBmXB2orS2KFIw9G2wfDsNBODfAorHH0IanAkyOR6hjhYjuuDK3c9ajCUwtBLLeWuQsvJ5HBmByMFmZsj8s7ylaB6IkluttGyeJuh0fFJ/98Pf2BtS7+LnArU0gSnrTh2OTWZU68DboBRtIt44XgVgK/Y3Ew2KH6jfhDNz1Snv/0US7bTSYfvUOti8oRKrFvA037Hqq4Yp4EkJObbeHIqV+h7fZA2yD+UA=";
            if (string.IsNullOrEmpty(key))
            {
                return KingResponse.GetErrorResponse("参数key为空!");
            }
            else if (string.IsNullOrEmpty(data))
            {
                return KingResponse.GetErrorResponse("参数data为空!");
            }
            CourseActivatKeyInfo parmModel = new CourseActivatKeyInfo();
            try
            {
                Manage manage = new Manage();
                CourseActivatKeyResult result = new CourseActivatKeyResult();
                string privatKey = ConfigItemHelper.PrivateKey;
                //RSA解密:最外层的key，得到明文key
                string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
                //des解密： 用明文key解密data，得到原数据datajson
                string dataStr = DESEncrypt.DecryptDES(data, keyStr);
                //json转对象
                parmModel = JsonHelper.FromJsonIgnoreNull<CourseActivatKeyInfo>(dataStr);

                if (parmModel != null)
                {
                    #region 时间验证
                    TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(parmModel.RTime));
                    //if (ts.Seconds > 15) return KingResponse.GetErrorResponse("请求接口失效");
                    #endregion

                    #region 激活码验证
                    ActivateCourseBLL bll = new ActivateCourseBLL();
                    KingResponse res = bll.ActivateCourse(parmModel.UID, parmModel.UName, parmModel.CourseID, parmModel.Code, parmModel.DeviceType, parmModel.DeviceCode);
                    //if (!res.Success) return res;
                    #endregion

                    #region 获取资源
                    SourceInfo model = new SourceInfo();
                    if (parmModel.Channel == 0)
                    {
                        //    //客户端资源                        
                        //    model.SourceMD5 = "123";
                        //    model.SourceURL = "123456";
                        //    model.SourceVersion = "1.1";
                        //    model.SourceKey = "1";
                    }
                    else
                    {
                        //同步学资源
                        model.SourceMD5 = "123";
                        model.SourceURL = "123456";
                        model.SourceVersion = "1.1";
                        model.SourceKey = "1";
                        model.PchKey = "kingsoftdiandu01";

                        manage.SynInsert<TB_UserMember>(new TB_UserMember
                        {
                            ID = Guid.NewGuid(),
                            CourseID = parmModel.CourseID,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(12),
                            UserID = parmModel.UID,
                            Months = 12,
                            CreateTime = DateTime.Now,
                            Status = 0,
                            TbOrderID = Guid.NewGuid()
                            //TbOrderID = TbOrderID
                        });

                    }
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
                    string keyTmp = "12345678";// CommonHelper.GetTmp10Str().Substring(0, 8);
                    //RSA加密后的key,客户端用parmModel.Key进行解密
                    string keyDes = RSAHelper.encryptData(keyTmp, parmModel.PKey, "UTF-8");
                    string strData = JsonHelper.ToJson(model);
                    //DES加密返回的data
                    string dataDes = DESEncrypt.EncryptDES(strData, keyTmp);
                    result.Key = keyDes;
                    result.Info = dataDes;
                    #endregion

                    return KingResponse.GetResponse(result);
                }
                else
                {
                    return KingResponse.GetErrorResponse("data解密失败");
                }

            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse("课程加密解密及激活验证失败" + ex.Message);
            }
        }
    }
}