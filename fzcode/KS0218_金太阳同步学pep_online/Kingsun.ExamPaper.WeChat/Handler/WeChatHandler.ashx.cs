using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.WeChat.api;
using Kingsun.ExamPaper.WeChat.RelationService;

namespace Kingsun.ExamPaper.WeChat.Handler
{
    /// <summary>
    /// WeChatHandler 的摘要说明
    /// </summary>
    public class WeChatHandler : IHttpHandler, IReadOnlySessionState
    {
        private readonly RelationService.RelationService _relationService = new RelationService.RelationService();
        private FZUUMS_UserService.FZUUMS_UserService userservice = new FZUUMS_UserService.FZUUMS_UserService();
        readonly PhoneManage _phonemange = new PhoneManage();
        private BaseManagement _bm = new BaseManagement();
        readonly string _appId = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        private readonly BookDAL _bookdal = new BookDAL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string queryKey = context.Request["queryKey"].ToLower();
            switch (queryKey)
            {
                case "getuserclassbyuserid":
                    GetUserClassByUserId(context);
                    break;
                case "teabindclass":
                    TeaBindClass(context);
                    break;
                case "getuserinfobyuserid":
                    GetUserInfoByUserId(context);
                    break;
                case "loginbyphone":
                    LoginByPhone(context);
                    break;
                case "sendcode":
                    SendCode(context);
                    break;
                case "updateuserinfo":
                    UpdateUserInfo(context);
                    break;
                case "getstulistbyclassid":
                    GetStuListByClassID(context);
                    break;
                case "getunitbygradeid":
                    GetUnitByGradeID(context);
                    break;
                case "getuserinfobytelephone":
                    GetUserInfoByTelephone(context);
                    break;
                case "getcatalogbybookid":
                    GetCatalogByBookID(context);
                    break;
                default:
                    context.Response.Write("{\"Result\":\"false\",\"msg\":\"\",\"data\":\"\"}");
                    break;
            }
        }
        CatalogBLL catalogBLL = new CatalogBLL();
        /// <summary>
        /// 根据BookID得到目录信息
        /// </summary>
        /// <param name="context"></param>
        private void GetCatalogByBookID(HttpContext context)
        {
            try
            {
                int BooKID = Convert.ToInt32(context.Request.Form["BooKID"]);
                DataSet set = catalogBLL.GetCatalogByBookID(BooKID);
                if (set != null)
                {
                    List<CatalogParentInfo> list = new List<CatalogParentInfo>();
                    list = JsonHelper.DataSetToIList<CatalogParentInfo>(set, 0);
                    var obj = new { Success = true, Data = list, Msg = "" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    var obj = new { Success = true, Data = "", Msg = "" };
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
        /// 通过老师ID获取老师绑定班级信息
        /// </summary>
        /// <param name="context"></param>
        private void GetUserClassByUserId(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            try
            {

                ReturnInfo classList = _relationService.GetTeacherClassInfoByUserId(userId);
                List<ClassInfo> returnList = new List<ClassInfo>();

                if (classList != null)
                {
                    if (classList.Data != null)
                    {
                        List<string> arr = new List<string>();
                        CSinfo csinfo = JsonHelper.DecodeJson<CSinfo>(classList.Data.ToString());
                        foreach (var item in csinfo.ClassInfo)
                        {
                            ClassInfo cInfo = new ClassInfo
                            {
                                StudentNum = 0,
                                Id = item.Id,
                                ClassNum = item.ClassNum,
                                ClassName = item.ClassName,
                                SchoolId = item.SchoolId,
                                GradeId = item.GradeId
                            };
                            arr.Add(item.Id);
                            returnList.Add(cInfo);
                        }
                        returnList = ClassOrder(returnList);
                        ReturnInfo studentNum = _relationService.GetUserClassStudentSum(arr.ToArray());
                        if (studentNum.Success)
                        {
                            List<ClassNumInfo> ci = JsonHelper.DecodeJson<List<ClassNumInfo>>(studentNum.Data.ToString());
                            foreach (ClassInfo t in returnList)
                            {
                                foreach (ClassNumInfo t1 in ci)
                                {
                                    if (t.Id == t1.ClassID)
                                    {
                                        t.StudentNum = Convert.ToInt32(t1.UserCount);
                                    }
                                }
                            }
                        }

                        var obj = new { Success = classList.Success, ClassList = returnList, Msg = classList.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = classList.Success, Msg = classList.ErrorMsg };
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
        /// 老师绑定班级
        /// </summary>
        /// <param name="context"></param>
        private void TeaBindClass(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            string schoolId = context.Request.Form["SchoolID"];
            string classStr = context.Request.Form["ClassStr"];
            RelationService.RelationService relationService = new RelationService.RelationService();
            ReturnInfo userinfo = relationService.GetUserInfoByUserID(userId);
            TB_UUMSUser uumsUser = new TB_UUMSUser();
            string re = "";
            if (userinfo != null)
            {
                uumsUser = JsonHelper.DecodeJson<TB_UUMSUser>(userinfo.Data.ToString());
            }

            bool flag = false;
            if (classStr.IndexOf(',') > 0)
            {
                int num = 0;
                string[] classArr = classStr.Split(',');
                for (int i = 0, length = classArr.Length; i < length; i++)
                {
                    ReturnInfo result = relationService.AddClassInCPoint(schoolId, classArr[i], userId);
                    if (result != null)
                    {
                        if (result.Data != null)
                        {
                            string classid = result.Data.ToString();
                            if (userinfo != null)
                            {
                                if (userinfo.Success)
                                {
                                    if (uumsUser.UserType == 12)
                                    {
                                        result = relationService.AddUserClass(userId, classid, schoolId, "3");
                                    }
                                    if (result != null)
                                    {
                                        if (result.Success)
                                        {
                                            //UserBindLocalClass(classid, userId);
                                        }
                                        else
                                        {
                                            num++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                flag = num <= 0;
            }
            else
            {
                ReturnInfo result = relationService.AddClassInCPoint(schoolId, classStr, userId);
                if (result != null)
                {
                    string classid = result.Data.ToString();
                    if (uumsUser.UserType == 12)
                    {
                        result = relationService.AddUserClass(userId, classid, schoolId, "3");
                    }
                    flag = result.Success;
                }
            }
            var obj = new { Success = flag, Result = re, Msg = "无法创建班级" };
            context.Response.Write(JsonHelper.EncodeJson(obj));
            context.Response.End();
        }

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="context"></param>
        private void GetUserInfoByUserId(HttpContext context)
        {
            string userId = context.Request.Form["UserID"];
            RelationService.RelationService relationService = new RelationService.RelationService();

            ReturnInfo result = relationService.GetUserInfoByUserID(userId);
            if (result.Success)
            {
                TB_UUMSUser userInfo = JsonHelper.DecodeJson<TB_UUMSUser>(result.Data.ToString());
                var obj = new { Success = true, UserInfo = userInfo };
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
        /// 手机登陆
        /// </summary>
        /// <param name="context"></param>
        private void LoginByPhone(HttpContext context)
        {
            string telephone = context.Request.Form["Telephone"];
            string openId = context.Request.Form["OpenId"];
            string code = context.Request.Form["Code"];
            int type = int.Parse(context.Request.Form["Type"].ToString());
            FZUUMS_UserService.FZUUMS_UserService UUMSService = new FZUUMS_UserService.FZUUMS_UserService();
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

            if (_phonemange.CheckPhoneCode(telephone, code))
            {
                FZUUMS_UserService.ReturnInfo returnInfo = UUMSService.TBXLoginByPhone(_appId, telephone, type);
                string userId = returnInfo.Data.ToString().Split('|')[0];
                string userName = returnInfo.Data.ToString().Split('|')[1];

                if (returnInfo.Data != null)
                {
                    FZUUMS_UserService.User fzUser = new FZUUMS_UserService.User
                    {
                        UserID = userId,
                        UserType = 12
                    };
                    FZUUMS_UserService.ReturnInfo ri = UUMSService.UpdateUserInfo2(_appId, fzUser);

                    UserOpenBLL userBll = new UserOpenBLL();
                    string where = "OpenID='" + openId + "'";
                    TB_UserOpenID openInfo = userBll.GetPhoneByOpenId(where);
                    bool updatebool = false;
                    bool insertbool = false;
                    if (openInfo != null)
                    {
                        TB_UserOpenID uoID = new TB_UserOpenID
                        {
                            ID = openInfo.ID,
                            TelePhone = telephone,
                            UserID = Convert.ToInt32(userId),
                            OpenID = openId,
                            CreateDate = DateTime.Now
                        };
                        updatebool = _bm.Update<TB_UserOpenID>(uoID);

                    }
                    else
                    {
                        TB_UserOpenID uoID = new TB_UserOpenID
                        {
                            TelePhone = telephone,
                            UserID = Convert.ToInt32(userId),
                            OpenID = openId,
                            CreateDate = DateTime.Now
                        };
                        insertbool = _bm.Insert<TB_UserOpenID>(uoID);

                    }

                    var obj = new { Success = true, UserID = userId, ib = insertbool, ub = updatebool };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
                else
                {
                    var obj = new { Success = false, UserID = userId };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
            else
            {
                var obj = new { Success = false, Msg = "验证码错误！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 向手机发送验证码
        /// </summary>
        /// <param name="context"></param>
        private void SendCode(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                int EndMessageCodeTime = 5;

                string telephone = context.Request.Form["Telephone"].ToString();
                if (string.IsNullOrEmpty(telephone))
                {
                    test t = new test() { Success = false, Msg = "电话号码错误!" };
                    //var returnObj = new { Success = false, Msg = "电话号码错误!" };
                    context.Response.Write(JsonHelper.EncodeJson(t));
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
                    Tb_PhoneCode phonecode = new Tb_PhoneCode();
                    phonecode.Code = CommonHelper.RndNumRNG(6);
                    phonecode.EndDate = DateTime.Now.AddMinutes(EndMessageCodeTime);
                    phonecode.TelePhone = telephone.Trim();
                    phonecode.State = 1;
                    if (_phonemange.InInsert(phonecode))
                    {
                        //string checkCode = Utils.Number(6);
                        //验证码缓存起来 为5分钟有效
                        if (context.Cache[telephone] != null)
                        {
                            context.Cache.Remove(telephone);
                        }
                        context.Cache.Insert(telephone, telephone + "," + phonecode.Code, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration); //这里给数据加缓存，设置缓存时间
                        SMSService.SMSService smssmessage = new SMSService.SMSService();
                        string MessageContent = "您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                        string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, MessageContent);

                        string[] resultArr = results.Split(new char[] { ',' });
                        if (resultArr[0] == "0")
                        {
                            //CheckCode = phonecode.Code, TelePhone = telephone,
                            test t = new test { Success = true, Msg = "电话号码错误!" };
                            //var returnObj = new {  Success = true, Msg = "" };
                            context.Response.Write(serializer.Serialize(t));
                        }
                        else
                        {
                            var returnObj = new { Success = false, Msg = "验证码发送失败!" };
                            context.Response.Write(JsonHelper.EncodeJson(returnObj));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                test t = new test { Success = true, Msg = "错误" + ex };
                context.Response.Write(serializer.Serialize(t));
                //var returnObj = new { Success = false, Msg = ex };
                //context.Response.Write(JsonHelper.EncodeJson(returnObj));
            }
            context.Response.End();
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

                    RelationService.RelationService relationService = new RelationService.RelationService();

                    ReturnInfo result = relationService.UpdateUserInfo(userId, trueName, schoolId, schoolName, subjectids, subjectName);

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
        /// 通过班级ID获取班级信息
        /// </summary>
        /// <param name="context"></param>
        private void GetStuListByClassID(HttpContext context)
        {
            string classID = context.Request.Form["ClassID"].ToString();
            RelationService.RelationService relationService = new RelationService.RelationService();
            tb_Relation[] studentList = relationService.GetStuListByClassID(classID);
            List<UserNameOrTrueName> tu = new List<UserNameOrTrueName>();
            UserNameOrTrueName unot = new UserNameOrTrueName();
            if (studentList != null && studentList.Length > 0)
            {
                string studentStr = "";
                for (int i = 0, length = studentList.Length; i < length; i++)
                {
                    ReturnInfo result = new ReturnInfo();
                    try
                    {
                        Guid.Parse(studentList[i].OtherID);
                        result = relationService.GetUserInfoByUserID(studentList[i].SelfID);
                    }
                    catch (Exception)
                    {
                        result = relationService.GetUserInfoByUserID(studentList[i].OtherID);
                    }
                    if (result.Success)
                    {
                        TB_UUMSUser userInfo = JsonHelper.DecodeJson<TB_UUMSUser>(result.Data.ToString());
                        if (userInfo != null)
                        {
                            unot.TrueName = userInfo.TrueName;
                            unot.UserName = userInfo.UserName;
                            tu.Add(unot);
                        }
                    }
                }
                var obj = new { Success = true, ClassList = tu };
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
        /// 通过年纪ID获取对应书籍
        /// </summary>
        /// <param name="context"></param>
        private void GetUnitByGradeID(HttpContext context)
        {
            string GradeID = context.Request.Form["GradeID"].ToString();
            List<QTb_Book> list = new List<QTb_Book>();
            IList<QTb_Book> qtbbook = _bookdal.GetBookList("GradeID=" + GradeID + " AND IsRemove=0");
            if (qtbbook != null && qtbbook.Count > 0)
            {
                foreach (var item in qtbbook)
                {
                    QTb_Book bk = new QTb_Book
                    {
                        BookID = item.BookID,
                        BookName = item.BookName,
                        EditionID = item.EditionID
                    };
                    list.Add(bk);
                }
                var obj = new { Success = true, Book = list };
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
        /// 获取用户基本信息通过手机号
        /// </summary>
        /// <param name="context"></param>
        private void GetUserInfoByTelephone(HttpContext context)
        {
            string Telephone = context.Request.Form["Telephone"].ToString();

            FZUUMS_UserService.User user = userservice.GetUserInfoByTelephone(_appId, Telephone);
            if (user != null)
            {
                var obj = new { Success = true, UserInfo = user };
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
        /// 班级排序
        /// </summary>
        /// <param name="list"></param>
        private List<ClassInfo> ClassOrder(List<ClassInfo> list)
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

        public class UserNameOrTrueName
        {
            public string TrueName { get; set; }
            public string UserName { get; set; }

        }

        public class test
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
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

        public class CSinfo
        {
            public ClassInfo[] ClassInfo;
            public SchoolInfo SchoolInfo;
        }

        public class SchoolInfo
        {
            public int? SchoolId { get; set; }
            public string SchoolName { get; set; }
        }

        public class ClassNumInfo
        {
            public string ClassID { get; set; }
            public string UserCount { get; set; }
        }
        public class CatalogParentInfo
        {
            public int? CatalogID { get; set; }
            public string CatalogName { get; set; }
            public string ParentCatalogName { get; set; }
            public int? ParentID { get; set; }
            /// <summary>
            /// 学习人数
            /// </summary>
            public int? Num { get; set; }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}