using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Web.LedgerRelation;
using log4net;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// WeChatHandler 的摘要说明
    /// </summary>
    public class WeChatHandler : IHttpHandler, IReadOnlySessionState
    {
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string FoundationDatabase = WebConfigurationManager.AppSettings["FounDation"]; //基础数据库
        string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        PhoneManage phonemange = new PhoneManage();

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string queryKey = context.Request["queryKey"].ToLower();
            switch (queryKey)
            {
                case "sendcode":
                    SendCode(context);
                    break;
                case "getlocation":
                    GetLocation(context);
                    break;
                case "getclassbytelephone":
                    GetClassByTelePhone(context);
                    break;
                case "queryclasslist":
                    QueryClassList(context);
                    break;
                case "deleteclasslist":
                    DeleteClassList(context);
                    break;
                case "deletestudentlist":
                    DeleteStudentList(context);
                    break;
                case "getstulistbyclassid":
                    GetStuListByClassID(context);
                    break;
                case "getuserinfo":
                    GetUserInfo(context);
                    break;
                case "updateuserinfo":
                    UpdateUserInfo(context);
                    break;
                case "teabindclass":
                    TeaBindClass(context);
                    break;
                case "studentbindclass":
                    StudentBindClass(context);
                    break;
                case "logout":
                    LogOut(context);
                    break;
                //case "updateuseraddinfo":
                //    UpdateUserAddInfo(context);
                //    break;
                case "getapplist":
                    GetAppList(context);
                    break;
                case "updateuserinfobyid":
                    UpdateUserInfoById(context);
                    break;
                case "insertopenid":
                    InsertOpenId(context);
                    break;
                case "getuseropenidinfo":
                    GetUserOpenIDInfo(context);
                    break;
                case "loginbyphone":
                    LoginByPhone(context);
                    break;
                case "getuserphone":
                    GetUserPhone(context);
                    break;
                case "updateversion":
                    UpdateVersion(context);
                    break;
                case "getuserversionname":
                    GetUserVersionName(context);
                    break;
                case "setuserversionname":
                    SetUserVersionName(context);
                    break;
                case "getusertypebyid":
                    GetUserTypeById(context);
                    break;
                case "getusername":
                    GetUserName(context);
                    break;
                case "updateislogstate":
                    UpdateIsLogState(context);
                    break;
                case "updateuserinfobylocal":
                    UpdateUserInfoByLocal(context);
                    break;
                case "getteaclasslistcount":
                    GetTeaClassListCount(context);
                    break;
                default:
                    context.Response.Write("{\"Result\":\"false\",\"msg\":\"\",\"data\":\"\"}");
                    break;
            }
        }

        #region
        /// <summary>
        /// 向手机发送验证码
        /// </summary>
        /// <param name="context"></param>
        private void SendCode(HttpContext context)
        {
            int EndMessageCodeTime = 5;

            string telephone = context.Request.Form["Telephone"];
            if (string.IsNullOrEmpty(telephone))
            {
                var returnObj = new { Success = false, Msg = "电话号码错误!" };
                context.Response.Write(JsonHelper.EncodeJson(returnObj));
                context.Response.End();
            }
            string sql = string.Format("  SELECT TOP 1 * FROM dbo.Tb_PhoneCode WHERE TelePhone='{0}'  AND EndDate>'{1}' AND State=1  ORDER BY EndDate DESC", telephone, DateTime.Now);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var returnObj = new { Success = false, Msg = "请使用五分钟内获取的验证码登陆!" };
                context.Response.Write(JsonHelper.EncodeJson(returnObj));
            }
            else
            {
                Tb_PhoneCode phonecode = new Tb_PhoneCode
                {
                    Code = CommonHelper.RndNumRNG(6),
                    EndDate = DateTime.Now.AddMinutes(EndMessageCodeTime)
                };
                if (telephone != null)
                {
                    phonecode.TelePhone = telephone.Trim();
                    if (phonemange.InInsert(phonecode))
                    {
                        //string checkCode = Utils.Number(6);
                        //验证码缓存起来 为5分钟有效
                        if (context.Cache[telephone] != null)
                        {
                            context.Cache.Remove(telephone);
                        }
                        context.Cache.Insert(telephone, telephone + "," + phonecode.Code, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration); //这里给数据加缓存，设置缓存时间
                        SMSService.SMSService smssmessage = new SMSService.SMSService();
                        string messageContent = ProjectConstant.MessageModel.Replace("[code]", phonecode.Code).Trim();//"您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                        string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, messageContent);
                        string[] resultArr = results.Split(',');
                        if (resultArr[0] == "0" || resultArr[0] == "200")
                        {
                            var returnObj =
                                new
                                {
                                    CheckCode = "",
                                    TelePhone = telephone,
                                    Success = true,
                                    Msg = ""
                                };
                            context.Response.Write(JsonHelper.EncodeJson(returnObj));
                        }
                        else
                        {
                            var returnObj = new { Success = false, Msg = "验证码发送失败!" };
                            context.Response.Write(JsonHelper.EncodeJson(returnObj));
                        }
                    }
                    else
                    {
                        var returnObj = new { Success = false, Msg = "验证码发送失败!" };
                        context.Response.Write(JsonHelper.EncodeJson(returnObj));
                    }
                }
            }
            context.Response.End();
        }

        /// <summary>
        /// 获取位置信息
        /// </summary>
        /// <param name="context"></param>
        private void GetLocation(HttpContext context)
        {
            const string ak = "oPT5n2xyDkn1nweS7GN5A0nCfIpa5GeO"; //百度地图访问应用（AK）
            const string url1 = "http://api.map.baidu.com/location/ip?ak=" + ak + "&coor=bd09ll";
            string json = HttpHelper.Get(url1);
            int start = json.IndexOf("point", StringComparison.Ordinal);
            int end = json.IndexOf("status", StringComparison.Ordinal);
            if (start > 0)
            {
                json = "{" + json.Substring(start + 8, end - start - 11);
                Point point = JsonHelper.DecodeJson<Point>(json);
                string url2 = "http://api.map.baidu.com/geocoder/v2/?callback=renderReverse&location=" + point.Y + ',' + point.X + "&output=json&pois=0&ak=" + ak;
                json = HttpHelper.Get(url2);
                var index = json.IndexOf("status", StringComparison.Ordinal);
                json = json.Substring(index - 2);
                json = json.Substring(0, json.Length - 1);
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write(json);
            }
            context.Response.End();
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="context"></param>
        private void LoginByPhone(HttpContext context)
        {
            string telephone = context.Request.Form["Telephone"];
            string code = context.Request.Form["Code"];
            string openid = context.Request.Form["OpenID"];

            if (phonemange.CheckPhoneCode(telephone, code))
            {
                var returnInfo = userBLL.TBXLoginByPhone(AppID, telephone, 26);
                string userId = returnInfo.Data.ToString().Split('|')[0];
                string userName = returnInfo.Data.ToString().Split('|')[1];

                var userInfo = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));

                //PSO.UUMSService.User info = UUMSService.GetUserInfoByID(AppID, userID);
                if (userInfo == null)
                {
                    Tb_UserInfo user = new Tb_UserInfo
                    {
                        UserID = string.IsNullOrEmpty(userId.ToString()) ? 0 : Convert.ToInt32(userId.ToString()),
                        UserName = userName,
                        TelePhone = telephone,
                        NickName = NickName(),
                        IsUser = 1,
                        IsEnableOss = 0,
                        isLogState = "0"
                    };
                    userBLL.Insert(user);
                }
                // RelationService.tb_Class[] classList = relationService.GetUserCreateClassListByUserId(userID);
                // context.Cache.Insert("user", telephone, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);  //这里给数据加缓存，设置缓存时间
                //SessionHelper.SetSession("phone", telephone);
                if (!string.IsNullOrEmpty(openid))
                {
                    string strSql = string.Format("SELECT Count(1) FROM TB_UserOpenID WHERE OpenID='{0}'", openid);
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strSql = string.Format("DELETE TB_UserOpenID WHERE OpenID='{0}'", openid);
                        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
                    }
                    string sql = string.Format(@"INSERT INTO TB_UserOpenID (Telephone,OpenID)VALUES('{0}','{1}')",
                        telephone, openid);
                    int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                    if (i > 0)
                    {
                        //var obj = new { Success = true, Msg = "绑定成功" };
                        //context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
                if (returnInfo.Data != null)
                {
                    var obj = new { Success = true, UserID = userId };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    var obj = new { Success = false, UserID = userId };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            else
            {
                var obj = new { Success = false, Msg = "验证码错误！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
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

        /// <summary>
        /// 通过手机号查询老师班级
        /// </summary>
        /// <param name="context"></param>
        private void GetClassByTelePhone(HttpContext context)
        {

            string telephone = context.Request.Form["Telephone"];
            string code = context.Request.Form["Code"];
            int type = int.Parse(context.Request.Form["Type"]);
            if (string.IsNullOrEmpty(code))
            {
                var obj = new { Success = false, Msg = "验证码不能为空！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            if (string.IsNullOrEmpty(telephone) || telephone == "undefined")
            {
                var obj = new { Success = false, Msg = "手机不能为空！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }

            if (phonemange.CheckPhoneCode(telephone, code))
            {
                var serviceInfo = userBLL.GetUserInfoByUserOtherID(telephone, 1);
                if (serviceInfo != null)
                {
                    var obj = new { Success = true, UserID = serviceInfo.UserID, UserType = serviceInfo.UserType };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    var returnInfo = userBLL.TBXLoginByPhone(AppID, telephone, type);
                    string userId = returnInfo.Data.ToString().Split('|')[0];
                    string userName = returnInfo.Data.ToString().Split('|')[1];
                    var userInfo = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                    //PSO.UUMSService.User info = UUMSService.GetUserInfoByID(AppID, userID);
                    if (userInfo == null)
                    {
                        Tb_UserInfo user = new Tb_UserInfo
                        {
                            UserID = string.IsNullOrEmpty(userId.ToString()) ? 0 : Convert.ToInt32(userId.ToString()),
                            UserName = userName,
                            IsUser = 1,
                            IsEnableOss = 0,
                            isLogState = "0"
                        };
                        userBLL.Insert(user);
                    }

                    SessionHelper.SetSession("phone", telephone);
                    if (returnInfo.Data != null)
                    {
                        var obj = new { Success = true, UserID = userId, UserType = userInfo.UserType };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = false, UserID = userId };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
            }
            else
            {
                var obj = new { Success = false, Msg = "验证码错误！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }

        /// <summary>
        /// 通过ID修改UserType
        /// </summary>
        /// <param name="context"></param>
        private void UpdateUserInfoById(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            int userType = Convert.ToInt32(context.Request.Form["UserType"]);
            IBS_UserInfo user = new IBS_UserInfo
            {
                UserID = Convert.ToInt32(userId),
                UserType = userType
            };

            //修改用户类型
            var returnInfo = userBLL.UpdateUserInfoNoOnly(AppID, user);
            if (returnInfo.Data != null)
            {
                var obj = new { Success = true, UserID = userId };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            else
            {
                var obj = new { Success = false, Msg = returnInfo.ErrorMsg };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
        }

        /// <summary>
        /// 通过老师ID
        /// </summary>
        /// <param name="context"></param>
        private void QueryClassList(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];

            try
            {

                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                List<ClassInfo> returnList = new List<ClassInfo>();

                if (user != null)
                {
                    if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                    {
                        user.ClassSchList.ForEach(a =>
                        {
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                            if (classinfo != null)
                            {
                                ClassInfo cInfo = new ClassInfo
                                {
                                    StudentNum = classinfo.ClassStuList.Count,
                                    Id = classinfo.ClassID,
                                    ClassNum = classinfo.ClassNum.ToString(),
                                    ClassName = classinfo.ClassName,
                                    SchoolId = classinfo.SchID
                                };
                                returnList.Add(cInfo);
                            }
                        }
                        );

                        returnList = ClassOrder(returnList);
                        var obj = new { Success = returnList.Count > 0, ClassList = returnList, Msg = "" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = returnList.Count > 0, ClassList = returnList, Msg = "" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
                else
                {
                    var obj = new { Success = false };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            catch (Exception ex)
            {
                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }

        /// <summary>
        /// 删除老师绑定班级
        /// </summary>
        /// <param name="context"></param>
        private void DeleteClassList(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            string subjectId = context.Request.Form["SubjectID"];
            string idStr = context.Request.Form["IDStr"];
            bool flag = false;

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
            if (user != null)
            {
                if (idStr.IndexOf(',') > 0)
                {
                    List<string> idArr = new List<string>(idStr.Split(','));
                    idArr.ForEach(a =>
                    {
                        var ss = classBLL.GetClassUserRelationByClassId(a);
                        if (ss != null)
                        {
                            if (ss.ClassStuList.Count > 0)
                            {
                                var objs = new { Success = false, ErrMsg = "不能删除有学生的班级!", Count = ss.ClassStuList.Count };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                context.Response.End();
                            }
                            UserClassData data = new UserClassData();
                            data.ClassID = a;
                            data.UserID = Convert.ToInt32(userId);
                            data.SubjectID = subjectId;
                            if (user.UserType == 12)
                            {

                                data.UserType = UserTypeEnum.Teacher;
                                var result = classBLL.UnBindClass(data);
                                if (result == null || !result.Success)
                                {
                                    var objs = new { Success = false, ErrMsg = result == null ? "" : result.ErrorMsg };
                                    context.Response.Write(JsonHelper.EncodeJson(objs));
                                    context.Response.End();
                                }

                                classBLL.UnBindClassByClassId(data.ClassID);
                                flag = true;
                            }
                            else if (user.UserType == 26)
                            {
                                data.UserType = UserTypeEnum.Student;
                                var result = classBLL.UnBindClass(data);
                                if (result == null || !result.Success)
                                {
                                    var objs = new { Success = false, ErrMsg = result == null ? "" : result.ErrorMsg };
                                    context.Response.Write(JsonHelper.EncodeJson(objs));
                                    context.Response.End();
                                }
                                classBLL.UnBindClassByClassId(data.ClassID);
                                flag = true;
                            }
                        }
                    });


                }
                else
                {
                    var ss = classBLL.GetClassUserRelationByClassId(idStr);
                    if (ss != null)
                    {
                        if (ss.ClassStuList.Count > 0)
                        {
                            var objs = new { Success = false, ErrMsg = "不能删除有学生的班级!", Count = ss.ClassStuList.Count };
                            context.Response.Write(JsonHelper.EncodeJson(objs));
                            context.Response.End();
                        }
                        UserClassData data = new UserClassData();
                        data.ClassID = idStr;
                        data.UserID = Convert.ToInt32(userId);
                        data.SubjectID = subjectId;
                        if (user.UserType == 12)
                        {

                            data.UserType = UserTypeEnum.Teacher;
                            var result = classBLL.UnBindClass(data);
                            if (result == null || !result.Success)
                            {
                                var objs = new { Success = false, ErrMsg = result == null ? "" : result.ErrorMsg };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                context.Response.End();
                            }
                            classBLL.UnBindClassByClassId(data.ClassID);
                            flag = true;
                        }
                        else if (user.UserType == 26)
                        {
                            data.UserType = UserTypeEnum.Student;
                            var result = classBLL.UnBindClass(data);
                            if (result == null || !result.Success)
                            {
                                var objs = new { Success = false, ErrMsg = result == null ? "" : result.ErrorMsg };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                context.Response.End();
                            }
                            classBLL.UnBindClassByClassId(data.ClassID);
                            flag = true;
                        }
                    }
                    else
                    {
                        var objs = new { Success = false, ErrMsg = "不存在此班级!", Count = 0 };
                        context.Response.Write(JsonHelper.EncodeJson(objs));
                        context.Response.End();
                    }

                }
                int count = 0;
                user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                if (user != null)
                {
                    count = user.ClassSchList.Count;
                }
                var obj = new { Success = flag, Count = count };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Count = 0 };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }


        }

        /// <summary>
        /// 通过班级ID获取班级信息
        /// </summary>
        /// <param name="context"></param>
        private void GetStuListByClassID(HttpContext context)
        {
            string classId = context.Request.Form["ClassID"];
            var clsssinfo = classBLL.GetClassUserRelationByClassId(classId);
            List<UserInfo> stuList = new List<UserInfo>();
            if (clsssinfo != null && clsssinfo.ClassStuList.Count > 0)
            {
                clsssinfo.ClassStuList.ForEach(a =>
                {
                    var ibsuser = userBLL.GetUserInfoByUserId(Convert.ToInt32(a.StuID));
                    if (ibsuser != null)
                    {
                        UserInfo user = new UserInfo
                        {
                            UserId = ibsuser.UserID.ToString(),
                            UserName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName,
                            TrueName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName,
                            Gender = ibsuser.ClassSchList.FirstOrDefault() == null ? 0 : ibsuser.ClassSchList.First().GradeID,


                        };
                        if (!string.IsNullOrEmpty(ibsuser.UserImage) && ibsuser.UserImage != "00000000-0000-0000-0000-000000000000")
                        {
                            user.AvatarUrl = ibsuser.IsEnableOss != 0 ? _getOssFilesUrl + ibsuser.UserImage : _getFilesUrl + "?FileID=" + ibsuser.UserImage;
                        }
                        else
                        {
                            user.AvatarUrl = "00000000-0000-0000-0000-000000000000";
                        }

                        user.NickName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName;
                        stuList.Add(user);
                    }
                });
                var obj = new { Success = true, ClassList = stuList };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            else
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="context"></param>
        private void GetUserInfo(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            Ledger_Relation ledgerRelation = new Ledger_Relation();
            try
            {
                var result = userBLL.GetUserInfoByUserId(userId.ToInt());
                if (result != null)
                {
                    APPManagementBLL appManagementBll = new APPManagementBLL();
                    TB_UserEditionInfo editionInfo = appManagementBll.GetUserEditionByID(userId);
                    CashBackInfo cashBackInfo = new CashBackInfo();
                    TB_APPManagement appInfo = new TB_APPManagement();
                    if (editionInfo != null)
                    {
                        appInfo = appManagementBll.GetAPPByEditionID(editionInfo.VersionID.ToString());
                        if (editionInfo.VersionID != null)
                        {
                            string json = ledgerRelation.GetSunValue(userId, (int)editionInfo.VersionID);
                            cashBackInfo = JsonHelper.DecodeJson<CashBackInfo>(json);
                        }
                    }
                    else
                    {
                        cashBackInfo.Teacash = "0";
                        cashBackInfo.Resmoney = "0";
                    }
                    TB_UUMSUser userInfo = new TB_UUMSUser();
                    //RelationService.Tb_UserTelInviTation[] inviteInfo = relationService.GetUserTelInviTationByUserIdOrInvNum("", userID);

                    //TODO:12-28添加修改，确定是否能用用户ID查询到版本ID，如果查询不到需要跳转到添加学校信息页面，让用户添加版本
                    userInfo.AvatarUrl = result.IsEnableOss != 0 ? _getOssFilesUrl + result.UserImage : _getFilesUrl + "?FileID=" + result.UserImage;
                    userInfo.UserID = result.UserID.ToString();
                    userInfo.UserName = result.UserName.ToString();
                    userInfo.TrueName = result.TrueName.ToString();
                    userInfo.Telephone = result.TelePhone;
                    userInfo.UserType = result.UserType;
                    userInfo.SchoolID = userInfo.SchoolID;
                    userInfo.SchoolName = userInfo.SchoolName;
                    userInfo.AppID = userInfo.AppID;
                    userInfo.PersonID = result.UserNum;
                    // }
                    var obj = new { Success = true, UserInfo = userInfo, CashBackInfo = cashBackInfo, AppInfo = appInfo };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    var obj = new { Success = false };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);

                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            finally
            {
                context.Response.End();
            }
        }

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="context"></param>
        private void GetUserName(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            string appid = context.Request.Form["APPID"];

            try
            {
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                if (user != null)
                {
                    TB_UUMSUser userInfo = new TB_UUMSUser();

                    if (!string.IsNullOrEmpty(user.TrueName))
                    {
                        userInfo.TrueName = user.TrueName;
                    }
                    userInfo.AvatarUrl = user.IsEnableOss != 0 ? _getOssFilesUrl + user.UserImage : _getFilesUrl + "?FileID=" + user.UserImage;
                    userInfo.SchoolID = user.SchoolID;
                    userInfo.SchoolName = user.SchoolName;
                    string vName = "";
                    string sql = string.Format(@"SELECT VersionName FROM dbo.TB_APPManagement WHERE id='{0}'", appid);
                    DataSet da = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    if (da.Tables[0].Rows.Count > 0)
                    {
                        vName = da.Tables[0].Rows[0]["VersionName"].ToString();
                    }
                    string personId = string.IsNullOrEmpty(user.UserNum) ? "您尚未认证，请加添加微信号（tbx010）进行验证" : user.UserNum;
                    userInfo.PersonID = personId;

                    var obj = new { Success = true, UserInfo = userInfo, VersionName = vName, IsEnableOss = user.IsEnableOss };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    var obj = new { Success = false };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);

                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            finally
            {
                context.Response.End();
            }
        }

        /// <summary>
        /// POST请求与获取结果
        /// </summary>
        public static string HttpPost(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (string.IsNullOrEmpty(encoding))
            {
                encoding = "UTF-8"; //默认编码
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="context"></param>
        private void UpdateUserInfo(HttpContext context)
        {
            try
            {
                string userId = context.Request.Form["UserID"];
                string trueName = context.Request.Form["TrueName"];
                string schoolId = context.Request.Form["SchoolID"];
                string schoolName = context.Request.Form["SchoolName"];
                if (Utils.filterSql(trueName))
                {
                    var obj = new { Success = false, Msg = "有SQL攻击嫌疑，请停止操作。" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    int[] subjectids = { 3 };
                    string[] subjectName = { "英语" };


                    var result = userBLL.UpdateUserInfo(userId, trueName, schoolId, schoolName, subjectids, subjectName);

                    if (result.Success)
                    {
                        var obj = new { Success = true };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = false, Msg = result.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
            }
            catch (Exception)
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            finally
            {
                context.Response.End();
            }
        }

        /// <summary>
        /// 更新用户信息(本地)
        /// </summary>
        /// <param name="context"></param>
        private void UpdateUserInfoByLocal(HttpContext context)
        {
            try
            {
                string userId = context.Request.Form["UserID"];
                string trueName = context.Request.Form["TrueName"];
                string schoolId = context.Request.Form["SchoolID"];
                string schoolName = context.Request.Form["SchoolName"];
                if (Utils.filterSql(trueName))
                {
                    var obj = new { Success = false, Msg = "有SQL攻击嫌疑，请停止操作。" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    string sql = string.Format("UPDATE ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo SET NickName='{0}' WHERE UserID='{1}'  AND IsUser=1", trueName, userId);
                    int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                    if (i > 0)
                    {
                        var obj = new { Success = true };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = false, Msg = "修改失败" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
            }
            catch (Exception)
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            finally
            {
                context.Response.End();
            }
        }

        /// <summary>
        /// 老师绑定班级
        /// </summary>
        /// <param name="context"></param>
        private void TeaBindClass(HttpContext context)
        {
            try
            {
                string userId = context.Request.Form["UserID"];
                string schoolId = context.Request.Form["SchoolID"];
                string classStr = context.Request.Form["ClassStr"];

                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                TB_UUMSUser uumsUser = new TB_UUMSUser();
                string re = "";
                if (user != null)
                {
                    if (classStr.IndexOf(',') > 0)
                    {
                        List<string> classArr = new List<string>(classStr.Split(','));

                        classArr.ForEach(a =>
                        {
                            //新增班级
                            IBS_ClassUserRelation adClass = new IBS_ClassUserRelation();
                            adClass.ClassName = a;
                            adClass.SchID = Convert.ToInt32(schoolId);
                            string classID = classBLL.Add(adClass, Convert.ToInt32(userId));
                            if (string.IsNullOrEmpty(classID))
                            {
                                var objs = new { Success = false, ErrMsg = "绑定失败，创建班级编号为空" };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }

                            //绑定班级
                            UserClassData data = new UserClassData();
                            data.UserID = Convert.ToInt32(userId);
                            data.ClassID = classID;

                            if (user.UserType == 12)
                            {
                                data.SubjectID = "3";
                                data.UserType = UserTypeEnum.Teacher;
                                data.SchoolId = Convert.ToInt32(schoolId);
                                var result = classBLL.AddUserToClass(data);
                                if (result == null || !result.Success)
                                {
                                    var objs = new { Success = false, ErrMsg = result == null ? "绑定失败！" : result.ErrorMsg, Result = data.ClassID };
                                    context.Response.Write(JsonHelper.EncodeJson(objs));
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                }
                            }
                            else if (user.UserType == 26)
                            {
                                data.UserType = UserTypeEnum.Student;
                                data.Type = ModRelationTypeEnum.StuClass;
                                data.message = "";
                                data.flag = BCPointEnum.Cpoint;
                                data.SchoolId = schoolId.ToInt();
                                var result = classBLL.AddUserToClass(data);
                                if (result == null || !result.Success)
                                {
                                    var objs = new { Success = false, ErrMsg = result == null ? "绑定失败！" : result.ErrorMsg, Result = data.ClassID };
                                    context.Response.Write(JsonHelper.EncodeJson(objs));
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                }
                            }
                        });

                    }
                    else
                    {
                        IBS_ClassUserRelation adClass = new IBS_ClassUserRelation();
                        adClass.ClassName = classStr;
                        adClass.SchID = Convert.ToInt32(schoolId);
                        string classID = classBLL.Add(adClass, Convert.ToInt32(userId));


                        //result = relationService.AddUserClass(userID, result.Data.ToString(), schoolID, "3");
                        UserClassData data = new UserClassData();
                        data.UserID = Convert.ToInt32(userId);
                        data.ClassID = classID;
                        if (user.UserType == 12)
                        {
                            data.SubjectID = "3";
                            data.UserType = UserTypeEnum.Teacher;
                            data.SchoolId = schoolId.ToInt();
                            var result = classBLL.AddUserToClass(data);
                            if (result == null || !result.Success)
                            {
                                var objs = new { Success = false, ErrMsg = result == null ? "绑定失败！" : result.ErrorMsg, Result = data.ClassID };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                        else if (user.UserType == 26)
                        {
                            data.UserType = UserTypeEnum.Student;
                            data.Type = ModRelationTypeEnum.StuClass;
                            data.message = "";
                            data.flag = BCPointEnum.Cpoint;
                            var result = classBLL.AddUserToClass(data);
                            if (result == null || !result.Success)
                            {
                                var objs = new { Success = false, ErrMsg = result == null ? "绑定失败！" : result.ErrorMsg, Result = data.ClassID };
                                context.Response.Write(JsonHelper.EncodeJson(objs));
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }

                    var tc = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));

                    var obj = new { Success = true, Result = re, Msg = "", Count = tc.ClassSchList.Count };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    var obj = new { Success = false, Result = re, Msg = "找不到该老师！" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "TeaBindClass");
            }

        }

        /// <summary>
        /// 获取老师绑定班级数
        /// </summary>
        /// <param name="context"></param>
        private void GetTeaClassListCount(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
            if (user != null)
            {
                var obj = new { Success = true, Count = user.ClassSchList.Count };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Count = 0, ErrorMessage = "找不到该老师！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }



        }

        /// <summary>
        /// 学生绑定班级
        /// </summary>
        /// <param name="context"></param>
        private void StudentBindClass(HttpContext context)
        {
            string classId = context.Request.Form["ClassID"];
            string studentId = context.Request.Form["StudentID"];
            string userId = context.Request.Form["UserID"];
            var userInfo = userBLL.GetUserInfoByUserId(Convert.ToInt32(studentId));
            if (userInfo != null)
            {
                if (userInfo.ClassSchList.Count > 0)
                {
                    userInfo.ClassSchList.ForEach(a =>
                    {
                        UserClassData data = new UserClassData();
                        data.UserID = Convert.ToInt32(studentId);
                        data.ClassID = classId;

                        data.UserType = UserTypeEnum.Student;
                        data.Type = ModRelationTypeEnum.StuClass;
                        data.message = "";
                        data.flag = BCPointEnum.Cpoint;
                        var flag = classBLL.UnBindClass(data);
                        if (flag != null)
                        {
                            if (flag.Success)
                            {
                                flag = classBLL.AddUserToClass(data);
                                if (flag == null || !flag.Success)
                                {
                                    var obj = new { Success = false, Msg = flag == null ? "绑定失败！" : flag.ErrorMsg };
                                    context.Response.Write(JsonHelper.EncodeJson(obj));
                                }
                            }
                            else
                            {
                                var obj = new { Success = false, Msg = flag == null ? "绑定失败！" : flag.ErrorMsg };
                                context.Response.Write(JsonHelper.EncodeJson(obj));
                            }

                            var obj1 = new { Success = true, Msg = "" };
                            context.Response.Write(JsonHelper.EncodeJson(obj1));
                        }
                        else
                        {
                            var obj = new { Success = false, Msg = "" };
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                        }
                    });
                }
                else
                {
                    UserClassData data = new UserClassData();
                    data.UserID = Convert.ToInt32(studentId);
                    data.ClassID = classId;

                    data.UserType = UserTypeEnum.Student;
                    data.Type = ModRelationTypeEnum.StuClass;
                    data.message = "";
                    data.flag = BCPointEnum.Cpoint;
                    var flag = classBLL.AddUserToClass(data);
                    if (flag == null || !flag.Success)
                    {
                        var obj = new { Success = false, Msg = flag == null ? "绑定失败" : flag.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }

                }
            }
            else
            {
                var obj = new { Success = false, Msg = "您还不是本校学生" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }

        /// <summary>
        /// 从班级删除学生
        /// </summary>
        /// <param name="context"></param>
        private void DeleteStudentList(HttpContext context)
        {
            string classId = context.Request.Form["ClassID"];
            string idStr = context.Request.Form["IDStr"];
            if (idStr.IndexOf(',') > 0)
            {
                List<string> idArr = new List<string>(idStr.Split(','));
                idArr.ForEach(a =>
                {
                    UserClassData data = new UserClassData();
                    data.UserID = Convert.ToInt32(a);
                    data.ClassID = classId;

                    data.UserType = UserTypeEnum.Student;
                    data.Type = ModRelationTypeEnum.StuClass;
                    data.message = "";
                    data.flag = BCPointEnum.Cpoint;
                    var flag = classBLL.UnBindClass(data);
                    if (flag == null || !flag.Success)
                    {
                        var obj = new { Success = false, Msg = flag == null ? "绑定失败" : flag.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }

                });
            }
            else
            {
                UserClassData data = new UserClassData();
                data.UserID = Convert.ToInt32(idStr);
                data.ClassID = classId;

                data.UserType = UserTypeEnum.Student;
                data.Type = ModRelationTypeEnum.StuClass;
                data.message = "";
                data.flag = BCPointEnum.Cpoint;
                var flag = classBLL.UnBindClass(data);
                if (flag == null || !flag.Success)
                {
                    var obj = new { Success = false, Msg = flag == null ? "绑定失败" : flag.ErrorMsg };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            var obj1 = new { Success = true };
            context.Response.Write(JsonHelper.EncodeJson(obj1));
            context.Response.End();
        }

        /// <summary>
        /// 查询用户OpenID
        /// </summary>
        /// <param name="context"></param>
        private void GetUserOpenIDInfo(HttpContext context)
        {
            string openid = context.Request.Form["OpenID"];
            UserOpenIDBLL userOpenIdbll = new UserOpenIDBLL();
            TB_UserOpenID openidInfo = userOpenIdbll.GetUserOpenIDInfo(openid);
            if (openidInfo != null)
            {
                //string sql = @"DELETE [TB_UserOpenID] WHERE OpenID='" + openid + "'";
                //SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                var obj = new { Success = true };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 注销用户信息，退出登录
        /// </summary>
        /// <param name="context"></param>
        private void LogOut(HttpContext context)
        {
            string openid = context.Request.Form["OpenID"];
            UserOpenIDBLL userOpenIdbll = new UserOpenIDBLL();
            TB_UserOpenID openidInfo = userOpenIdbll.GetUserOpenIDInfo(openid);
            int i = 0;
            if (openidInfo != null)
            {
                string sql = @"DELETE [TB_UserOpenID] WHERE OpenID='" + openid + "'";
                i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            }
            if (i > 0)
            {
                var obj = new { Success = true };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }


        }

        /// <summary>
        /// 获取App列表
        /// </summary>
        /// <param name="context"></param>
        private void GetAppList(HttpContext context)
        {
            APPManagementBLL appManagementBll = new APPManagementBLL();
            try
            {
                IList<TB_APPManagement> appList = appManagementBll.QueryAPPList();
                var obj = new { Success = true, AppList = appList };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception)
            {
                var obj = new { Success = false };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            finally
            {
                context.Response.End();
            }

        }

        /// <summary>
        /// 获取手机号是否存在
        /// </summary>
        /// <param name="context"></param>
        public void GetUserPhone(HttpContext context)
        {
            string telephone = context.Request.Form["Telephone"];
            if (string.IsNullOrEmpty(telephone))
            {
                var obj = new { Success = false, Msg = "手机号为空" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
                return;
            }
            string sql = string.Format("SELECT  [Telephone] , [OpenID] FROM    [TB_UserOpenID] WHERE Telephone='{0}'", telephone);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var obj = new { Success = false, Msg = "手机号已存在" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = true, Msg = "手机号不存在" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 插入用户手机号、OpenID
        /// </summary>
        /// <param name="context"></param>
        public void InsertOpenId(HttpContext context)
        {
            string telephone = context.Request.Form["Telephone"];
            string openid = context.Request.Form["OpenID"];
            if (string.IsNullOrEmpty(telephone))
            {
                var obj = new { Success = false, Msg = "手机号为空" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
                return;
            }
            if (string.IsNullOrEmpty(openid))
            {
                var obj = new { Success = false, Msg = "openid为空" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
                return;
            }
            //try
            //{
            UserOpenIDBLL userOpenIdbll = new UserOpenIDBLL();
            TB_UserOpenID openidInfo = new TB_UserOpenID();

            string sql = string.Format("SELECT  [Telephone] , [OpenID] FROM    [TB_UserOpenID] WHERE Telephone='{0}'", telephone);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sql = "DELETE [TB_UserOpenID] WHERE Telephone='" + telephone + "'";
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            }

            openidInfo = userOpenIdbll.GetUserOpenIDInfo(openid);
            if (openidInfo == null)
            {
                var returnInfo = userBLL.TBXLoginByPhone(AppID, telephone, 26);
                string userId = returnInfo.Data.ToString().Split('|')[0];
                string userName = returnInfo.Data.ToString().Split('|')[1];

                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));
                if (user == null)
                {
                    Tb_UserInfo tbuser = new Tb_UserInfo
                    {
                        UserID = string.IsNullOrEmpty(userId.ToString()) ? 0 : Convert.ToInt32(userId.ToString()),
                        UserName = userName,
                        IsUser = 1,
                        IsEnableOss = 0,
                        isLogState = "0"
                    };
                    userBLL.Insert(tbuser);
                }
                SessionHelper.SetSession("phone", telephone);
                SessionHelper.SetSession("user", openid);
                if (!string.IsNullOrEmpty(openid))
                {
                    sql = string.Format(@"INSERT INTO TB_UserOpenID (Telephone,OpenID)VALUES('{0}','{1}')", telephone, openid);
                    int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                    if (i < 0)
                    {
                        var obj = new { Success = false, Msg = "绑定失败" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                        context.Response.End();
                    }
                    else
                    {
                        var obj = new { Success = true, Msg = "绑定成功！" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                        context.Response.End();
                    }
                }
                //TB_UserOpenID newInfo = new TB_UserOpenID();
                //newInfo.Telephone = telephone;
                //newInfo.OpenID = openid;
                //newInfo.CreateDate = DateTime.Now;

                //bool result = userOpenIDBLL.AddUserOpenIDInfo(newInfo);
                //if (result)
                //{
                //    var obj = new { Success = true, Msg = "绑定成功！" };
                //    context.Response.Write(JsonHelper.EncodeJson(obj));
                //    context.Response.End();
                //}
                //else
                //{
                //    var obj = new { Success = false, Msg = "手机与微信号绑定失败" };
                //    context.Response.Write(JsonHelper.EncodeJson(obj));
                //    context.Response.End();
                //}
            }
            else
            {
                openidInfo.Telephone = telephone;
                openidInfo.OpenID = openid;
                openidInfo.CreateDate = DateTime.Now;
                bool result = userOpenIdbll.UpdateOpenIDInfo(openidInfo);
                if (result)
                {
                    var obj = new { Success = true, Msg = "" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
                else
                {
                    var obj = new { Success = false, Msg = "更换绑定用户失败" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    var obj = new { Success = false, Msg = ex.Message };
            //    context.Response.Write(JsonHelper.EncodeJson(obj));
            //    context.Response.End();
            //}
        }



        /// <summary>
        /// 班级排序
        /// </summary>
        /// <param name="list"></param>
        public List<ClassInfo> ClassOrder(List<ClassInfo> list)
        {
            List<ClassInfo> classList = list;
            List<ClassInfo> returnList = new List<ClassInfo>();
            if (classList != null && classList.Count > 0)
            {
                string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                for (int i = 0, length = gradeArr.Length; i < length; i++)
                {
                    returnList.AddRange(classList.Where(t => t.ClassName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                }
            }
            return returnList;
        }

        /// <summary>
        /// 通过ID修改UserType
        /// </summary>
        /// <param name="context"></param>
        private void UpdateVersion(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            int versionId = Convert.ToInt32(context.Request.Form["VersionID"]);

            string sql = string.Format(@"UPDATE dbo.TB_UserEditionInfo SET VersionID='{0}' WHERE UserID='{1}'", versionId, userId);

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (i > 0)
            {
                var obj = new { Success = true, Msg = "修改成功" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Msg = "修改失败" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 通过ID修改IsLogState
        /// </summary>
        /// <param name="context"></param>
        private void UpdateIsLogState(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];

            string sql = string.Format(@"UPDATE ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo SET isLogState='1' WHERE UserID='{0}'  AND IsUser=1", userId);

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (i > 0)
            {
                var obj = new { Success = true, Msg = "修改成功" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Msg = "修改失败" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 获取用户APP版本
        /// </summary>
        /// <param name="context"></param>
        private void GetUserVersionName(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];

            string sql = string.Format(@"SELECT  a.VersionID ,
                                                b.VersionName,
                                                a.UserID
                                        FROM    dbo.TB_UserEditionInfo a
                                                LEFT JOIN dbo.TB_APPManagement b ON b.VersionID = a.VersionID WHERE a.UserID='{0}'", userId);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var obj = new { Success = true, VersionName = ds.Tables[0].Rows[0]["VersionName"].ToString(), VersionID = ds.Tables[0].Rows[0]["VersionID"].ToString(), Msg = "修改成功" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            else
            {
                var obj = new { Success = false, Msg = "修改失败" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
        }

        /// <summary>
        /// 通过UserID设置用户版本
        /// </summary>
        /// <param name="context"></param>
        private void SetUserVersionName(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            string versionId = context.Request.Form["VersionID"];

            string sql = string.Format(@"SELECT * FROM dbo.TB_UserEditionInfo WHERE UserID='{0}'", userId);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sql = string.Format(@"UPDATE dbo.TB_UserEditionInfo SET VersionID='{0}' WHERE UserID='{1}'", versionId, userId);
                int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (i > 0)
                {
                    var obj = new { Success = true, Msg = "修改成功" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
                else
                {
                    var obj = new { Success = false, Msg = "修改失败" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
            else
            {
                sql = string.Format(@" INSERT  INTO dbo.TB_UserEditionInfo
                                                ( UserID, VersionID, CreateDate )
                                        VALUES  ( '{0}', '{1}', GETDATE() )", userId, versionId);
                int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (i > 0)
                {
                    var obj = new { Success = true, Msg = "添加成功" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
                else
                {
                    var obj = new { Success = false, Msg = "添加失败" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
        }

        /// <summary>
        /// 获取用户身份
        /// </summary>
        /// <param name="context"></param>
        private void GetUserTypeById(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userId));

            if (user != null)
            {

                var obj = new { Success = true, UserType = user.UserType, ClassList = true };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Msg = "用户身份不存在！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }



        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class UserTypeInfo
    {
        public string UserType { get; set; }
    }

    //经纬坐标值
    public class Point
    {
        public string X { get; set; }
        public string Y { get; set; }
    }

    //班级信息
    public class ClassInfo
    {
        public string Id { get; set; }
        public string ClassNum { get; set; }
        public string ClassName { get; set; }
        public int StudentNum { get; set; }
        public int? SchoolId { get; set; }
        public int GradeId { get; set; }
    }

    public class TeaInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string HeadImage { get; set; }
        public string ClassName { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }

    }

    //用户信息
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string TrueName { get; set; }
        public int? Gender { get; set; }
        public string NickName { get; set; }
    }

    //老师阳光值信息
    public class CashBackInfo
    {
        public bool Success { get; set; }
        public string TeacherId { get; set; }
        public string EditId { get; set; }
        public string Teacash { get; set; } //返现比例
        public string Resmoney { get; set; } //老师总金额
    }

    public class CSinfo
    {
        public ClassInfo[] ClassInfo;
        public SchoolInfo SchoolInfo;
    }

    public class ClassNumInfo
    {
        public string ClassID { get; set; }
        public string UserCount { get; set; }
    }

    public class SchoolInfo
    {
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}