using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Models;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using log4net;
using System.Reflection;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Newtonsoft.Json;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class ClassInfoController : ApiController
    {
        readonly BaseManagementOther _bmBaseDB = new BaseManagementOther();
        private readonly string _appId = WebConfigurationManager.AppSettings["AppID"];
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        private readonly string _foundationDatabase = WebConfigurationManager.AppSettings["FounDation"]; //基础数据库
        readonly BaseManagement _bm = new BaseManagement();
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();

        /// <summary>
        /// 根据Userid获取学校班级信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserSchoolAndClass([FromBody]KingRequest request)
        {
            ClassModel submitData = JsonHelper.DecodeJson<ClassModel>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("信息未传递过来");
            }
            if (submitData.UserID == null)
            {
                return ObjectToJson.GetErrorResult("用户信息未传递过来");
            }
            try
            {
                //根据UserID查询用户信息
                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(submitData.UserID));
                if (user != null)
                {
                    if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Teacher)
                    {
                        if (user.ClassSchDetailList != null&&user.ClassSchDetailList.Count>0)
                        {
                            var tc = user.ClassSchDetailList.Last();
                            return GetUserSchoolInfo(tc.ClassID, tc.SchID, tc.ClassName);
                        }
                        else
                        {
                            object obj = new
                            {
                                ClassID = " ",
                                ClassName = " ",
                                SchoolID = " ",
                                SchoolName = " "
                            };
                            return ObjectToJson.GetResult(obj);
                            //return ObjectToJson.GetErrorResult("有耕耘才有收获，创建班级后再来试试～");
                        }
                    }
                    else if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Student)
                    {
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            var tc = user.ClassSchDetailList.First();
                            return GetUserSchoolInfo(tc.ClassID.ToString(), tc.SchID, tc.ClassName);
                        }
                        else
                        {
                            object obj = new
                            {
                                ClassID = " ",
                                ClassName = " ",
                                SchoolID = " ",
                                SchoolName = " "
                            };
                            return ObjectToJson.GetResult(obj);
                        }
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("当前用户身份不符！");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("用户不存在");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserSchoolAndClassTest()
        {
            string UserID = "1000191788";
            try
            {
                //根据UserID查询用户信息
                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(UserID));
                if (user != null)
                {
                    if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Teacher)
                    {
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            var tc = user.ClassSchDetailList.Last();
                            return GetUserSchoolInfo(tc.ClassID, tc.SchID, tc.ClassName);
                        }
                        else
                        {
                            object obj = new
                            {
                                ClassID = " ",
                                ClassName = " ",
                                SchoolID = " ",
                                SchoolName = " "
                            };
                            return ObjectToJson.GetResult(obj);
                            //return ObjectToJson.GetErrorResult("有耕耘才有收获，创建班级后再来试试～");
                        }
                    }
                    else if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Student)
                    {
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            var tc = user.ClassSchDetailList.First();
                            return GetUserSchoolInfo(tc.ClassID.ToString(), tc.SchID, tc.ClassName);
                        }
                        else
                        {
                            object obj = new
                            {
                                ClassID = " ",
                                ClassName = " ",
                                SchoolID = " ",
                                SchoolName = " "
                            };
                            return ObjectToJson.GetResult(obj);
                        }
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("当前用户身份不符！");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("用户不存在");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult("异常:" + ex.Message);
            }
        }

        /// <summary>
        /// 通过ID查询老师所在班级与学校信息
        /// </summary>
        /// <param name="rtInfo"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        private HttpResponseMessage GetUserSchoolInfo(string ClassID, int? SchoolID, string ClassName)
        {
            //根据学校ID查询学校信息
            string url = _foundationDatabase + "/service/SELSCHINFO.sun?ID=" + SchoolID;
            try
            {
                string htmls = HttpPost(url, "");
                KinResponses kingresponse = (KinResponses)JsonConvert.DeserializeObject(htmls, typeof(KinResponses));
                if (kingresponse.Success)
                {
                    if (kingresponse.Data != null)
                    {
                        SchoolInfo schoolInfo = JsonHelper.DecodeJson<SchoolInfo>(kingresponse.Data.ToString());
                        //  var schoolinfo = JsonHelper.DecodeJson<Schooles>(kingresponse.data.ToString());
                        object obj = new
                        {
                            ClassID = ClassID,
                            ClassName = ClassName,
                            SchoolID = schoolInfo.SchoolID,
                            SchoolName = schoolInfo.SchoolName
                        };
                        return ObjectToJson.GetResult(obj);
                    }
                    else
                    {
                        object obj = new
                        {
                            ClassID = ClassID,
                            ClassName = ClassName,
                            SchoolID = "",
                            SchoolName = ""
                        };
                        return ObjectToJson.GetResult(obj);
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("调用接口出错");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult("异常信息:" + ex.Message);
            }
        }

        /// <summary>
        /// 根据 ClassID 获取学校,班级信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetClassInfor([FromBody]KingRequest request)
        {
            ClassModel submitData = JsonHelper.DecodeJson<ClassModel>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("班级信息未传递过来");
            }
            if (submitData.ClassID == null)
            {
                return ObjectToJson.GetErrorResult("班级编号未传递过来");
            }
            string classID = "";
            string calssname = "";
            // string result = ledgerRelation.GetClassByClass(submitData.ClassID);
            var classinfo = classBLL.GetClassUserRelationByClassId(submitData.ClassID);
            if (classinfo != null)
            {
                classID = classinfo.ClassID;
                calssname = classinfo.ClassName;
                //根据学校ID获取学校信息。（mod接口）
                string url = _foundationDatabase + "/service/SELSCHINFO.sun?ID=" + classinfo.SchID.ToString();
                try
                {
                    string htmls = HttpPost(url, "");
                    KinResponses kingresponse = (KinResponses)JsonConvert.DeserializeObject(htmls, typeof(KinResponses));
                    if (kingresponse.Success)
                    {
                        Dictionary<string, string> x1 = JsonHelper.DecodeJson<Dictionary<string, string>>(kingresponse.Data.ToString());
                        //  var schoolinfo = JsonHelper.DecodeJson<Schooles>(kingresponse.data.ToString());
                        int schoolID = Convert.ToInt32(x1["SchoolID"]);
                        string schoolname = x1["SchoolName"];
                        object obj = new
                        {
                            ClassID = classID,
                            ClassName = calssname,
                            SchoolID = schoolID,
                            SchoolName = schoolname
                        };
                        return ObjectToJson.GetResult(obj);
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("调用接口出错");
                    }
                }
                catch (Exception ex)
                {
                    //return ObjectToJson.GetErrorResult("参数不正确");
                    return ObjectToJson.GetErrorResult(ex.Message);
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("班级信息不存在！");
            }
        }



        /// <summary>
        /// POST请求与获取结果
        /// </summary>
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }
       
        #region  MD5加密  string pswToSecurity(string strpsw)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strpsw">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public static string pswToSecurity(string strpsw)
        {
            if (!string.IsNullOrEmpty(strpsw) && strpsw.Length != 0)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strpsw, "MD5");
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MobilePhone([FromBody] KingRequest request)
        {
            PhoneManage phonemange = new PhoneManage();
            TB_UserInfoExtend uie = new TB_UserInfoExtend();
            UserInfoListNum uiln = new UserInfoListNum();
            UserPhone submitData = JsonHelper.DecodeJson<UserPhone>(request.Data);



            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("缺少数据");
            }
            if (submitData.Phone == null)
            {
                return ObjectToJson.GetErrorResult("手机号不能为空！");
            }
            if (submitData.MessageCode == null)
            {
                return ObjectToJson.GetErrorResult("验证码不能为空！");
            }
            //if (Convert.ToInt32(submitData.UserType) <= 0 || submitData.UserType == null)
            //{
            //    submitData.UserType = "26";
            //}
            try
            {

                if (!string.IsNullOrEmpty(submitData.Phone))
                {
                    if (phonemange.CheckPhoneCode(submitData.Phone, submitData.MessageCode))
                    {
                        uie.EquipmentID = submitData.EquipmentID;
                        uie.DeviceType = submitData.DeviceType;
                        uie.IPAddress = submitData.IPAddress;
                        uie.CreateDate = DateTime.Now;
                        int logintype = 0;
                        var tuser = userBLL.GetUserALLInfoByUserOtherID(submitData.Phone, 1);
                        if (tuser == null)
                        {
                            logintype = 1;//注册
                        }
                        var re = userBLL.TBXLoginByPhone(_appId, submitData.Phone, 1, uie,submitData.AppId);//无法判断是登录还是注册
                        if (re.Success)
                        {
                            string[] strs = re.Data.ToString().Split('|');

                            if (logintype == 0)
                            {
                                AppSetting.SetValidUserLogin(strs[0], ProjectConstant.AppID, submitData.Versions, submitData.DownloadChannel);
                            }
                            else
                            {
                                //注册时数据埋点
                                AppSetting.SetValidUserRegister(strs[0], ProjectConstant.AppID, submitData.Versions, submitData.DownloadChannel);
                            }

                            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(strs[0]));
                            if (user != null)
                            {
                                AppSetting.SetValidUserRecord(string.IsNullOrEmpty(strs[0]) ? "0" : strs[0]);
                                string img = "";
                                if (submitData.IsEnableOss == 0)
                                {
                                    img = user.iBS_UserInfo.UserImage;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(user.iBS_UserInfo.UserImage))
                                    {
                                        img = user.iBS_UserInfo.IsEnableOss == 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                                    }
                                    else
                                    {
                                        img = user.iBS_UserInfo.UserImage;
                                    }
                                }
                                uiln.UserType = user.iBS_UserInfo.UserType;
                                if (user.iBS_UserInfo.UserType == (int) UserTypeEnum.Teacher)
                                {
                                    if (string.IsNullOrEmpty(user.iBS_UserInfo.SchoolName))
                                    {
                                        uiln.needImproveSource = "true";
                                    }
                                    else
                                    {
                                        uiln.needImproveSource = "false";
                                    }
                                    uiln.SchoolID = user.iBS_UserInfo.SchoolID;
                                    uiln.SchoolName = user.iBS_UserInfo.SchoolName;

                                }
                                else
                                {
                                    if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                                    {
                                        var classinfo = user.ClassSchDetailList.First();
                                        uiln.SchoolID = classinfo.SchID;
                                        uiln.SchoolName = classinfo.SchName;
                                    }
                                    else
                                    {
                                        uiln.SchoolID = 0;
                                        uiln.SchoolName = "";
                                    }
                                }

                                uiln.UserID = string.IsNullOrEmpty(strs[0]) ? 0 : Convert.ToInt32(strs[0]);
                                uiln.UserName = strs[1];
                                if (strs.Length > 2)
                                {
                                    uiln.ClassNum = strs[2];
                                }
                              
                                uiln.TelePhone = submitData.Phone;
                                uiln.UserNum = "";
                                uiln.NickName = user.iBS_UserInfo.TrueName;
                                uiln.UserImage = img;
                                uiln.UserRoles = Convert.ToInt32(user.iBS_UserInfo.UserRoles);
                                uiln.TrueName = user.iBS_UserInfo.TrueName;
                                uiln.ComboInfo = new AccountController().QueryCombo(uiln.UserID.ToString());
                                AccountController account = new AccountController();
                                uiln.Token = account.AddToken(uiln.UserID.ToString(), submitData.EquipmentID);
                                return ObjectToJson.GetResult(uiln, "手机号已被注册！");

                            }

                        }
                        return ObjectToJson.GetErrorResult("登陆失败！"+re.ErrorMsg);
                    }
                    return ObjectToJson.GetErrorResult("验证码错误！");
                }
                return ObjectToJson.GetErrorResult("手机号为空！");
            }
            catch (Exception ex)
            {
                _log.Error("error", ex);
                return ObjectToJson.GetErrorResult("登陆失败"+ex.Message);
            }
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage MobilePhone()
        {
            
            PhoneManage phonemange = new PhoneManage();
            TB_UserInfoExtend uie = new TB_UserInfoExtend();
            UserInfoListNum uiln = new UserInfoListNum();
            UserPhone submitData = new UserPhone();//JsonHelper.DecodeJson<UserPhone>(request.Data);
            submitData.Phone = "13530450190";
            submitData.MessageCode = "093759";


            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("缺少数据");
            }
            if (submitData.Phone == null)
            {
                return ObjectToJson.GetErrorResult("手机号不能为空！");
            }
            if (submitData.MessageCode == null)
            {
                return ObjectToJson.GetErrorResult("验证码不能为空！");
            }
            //if (Convert.ToInt32(submitData.UserType) <= 0 || submitData.UserType == null)
            //{
            //    submitData.UserType = "26";
            //}
            try
            {

                if (!string.IsNullOrEmpty(submitData.Phone))
                {
                    if (phonemange.CheckPhoneCode(submitData.Phone, submitData.MessageCode))
                    {
                        uie.EquipmentID = submitData.EquipmentID;
                        uie.DeviceType = submitData.DeviceType;
                        uie.IPAddress = submitData.IPAddress;
                        uie.CreateDate = DateTime.Now;
                        var re = userBLL.TBXLoginByPhone(_appId, submitData.Phone, 1, uie, submitData.AppId);
                        if (re.Success)
                        {
                            string[] strs = re.Data.ToString().Split('|');
                            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(strs[0]));
                            if (user != null)
                            {
                                Log4Net.LogHelper.Info(strs.ToJson());
                                AppSetting.SetValidUserRecord(string.IsNullOrEmpty(strs[0]) ? "0" : strs[0]);
                                string img = "";
                                if (submitData.IsEnableOss == 0)
                                {
                                    img = user.iBS_UserInfo.UserImage;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(user.iBS_UserInfo.UserImage))
                                    {
                                        img = user.iBS_UserInfo.IsEnableOss == 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                                    }
                                    else
                                    {
                                        img = user.iBS_UserInfo.UserImage;
                                    }
                                }
                                uiln.UserType = user.iBS_UserInfo.UserType;
                                if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Teacher)
                                {
                                    if (!string.IsNullOrEmpty(user.iBS_UserInfo.SchoolName))
                                    {
                                        uiln.needImproveSource = "true";
                                    }
                                    else
                                    {
                                        uiln.needImproveSource = "false";
                                    }
                                    uiln.SchoolID = user.iBS_UserInfo.SchoolID;
                                    uiln.SchoolName = user.iBS_UserInfo.SchoolName;

                                }
                                else
                                {
                                    if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                                    {
                                        var classinfo = user.ClassSchDetailList.First();
                                        uiln.SchoolID = classinfo.SchID;
                                        uiln.SchoolName = classinfo.SchName;
                                    }
                                    else
                                    {
                                        uiln.SchoolID = 0;
                                        uiln.SchoolName = "";
                                    }
                                }

                                uiln.UserID = string.IsNullOrEmpty(strs[0]) ? 0 : Convert.ToInt32(strs[0]);
                                uiln.UserName = strs[1];
                                if (strs.Length > 2)
                                {
                                    uiln.ClassNum = strs[2];
                                }
                                uiln.TelePhone = submitData.Phone;
                                uiln.UserNum = "";
                                uiln.NickName = user.iBS_UserInfo.TrueName;
                                uiln.UserImage = img;
                                uiln.UserRoles = Convert.ToInt32(user.iBS_UserInfo.UserRoles);
                                uiln.TrueName = user.iBS_UserInfo.TrueName;
                                uiln.ComboInfo = new AccountController().QueryCombo(uiln.UserID.ToString());
                                AccountController account = new AccountController();
                                uiln.Token = account.AddToken(uiln.UserID.ToString(), submitData.EquipmentID);
                                return ObjectToJson.GetResult(uiln, "手机号已被注册！");

                            }

                        }
                        return ObjectToJson.GetErrorResult("登陆失败！"+re.ErrorMsg);
                    }
                    return ObjectToJson.GetErrorResult("验证码错误！");
                }
                return ObjectToJson.GetErrorResult("手机号为空！");
            }
            catch (Exception ex)
            {
                _log.Error("error", ex);
                return ObjectToJson.GetErrorResult("登陆失败！"+ex.Message);
            }
        }

        /// <summary>
        /// 获取随机2位字母+四位随机数
        /// </summary>
        /// <returns></returns>
        public string NickName()
        {
            string[] s1 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            Random rand = new Random();

            return "同步学" + s1[rand.Next(0, s1.Length)] + s1[rand.Next(0, s1.Length)] + rand.Next(0, 9999);
        }

        public class UserPhone
        {
            public string Phone { get; set; }
            public string EquipmentID { get; set; }
            public string DeviceType { get; set; }
            public string IPAddress { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string MessageCode { get; set; }
            public string UserType { get; set; }
            public int IsEnableOss { get; set; }
            public string AppId { get; set; }
            /// <summary>
            /// app版本
            /// </summary>
            public string Versions { get; set; }

            /// <summary>
            /// 下载渠道
            /// </summary>
            public int DownloadChannel { get; set; }
        }

        public class CSinfo
        {
            public ClassInfo[] ClassInfo;
            public SchoolInfo SchoolInfo;
        }

        public class ClassInfo
        {
            public string ID { get; set; }
            public string ClassName { get; set; }
        }

        public class SchoolInfo
        {
            public string SchoolID { get; set; }
            public string SchoolName { get; set; }
        }

        public class UserInfo
        {
            public string SchoolID { get; set; }
            public string SchoolName { get; set; }
            public string ID { get; set; }
            public string ClassName { get; set; }
        }
    }

}
