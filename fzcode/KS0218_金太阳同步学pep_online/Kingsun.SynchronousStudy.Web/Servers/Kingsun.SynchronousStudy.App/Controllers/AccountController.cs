using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Models;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;
using System.Web.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.BLL;
using System.Data;
using System.Reflection;
using Kingsun.SynchronousStudy.App.Filter;
using System.Collections;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using log4net;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 账号相关接口
    /// </summary>
    public class AccountController : ApiController
    {
        int EndMessageCodeTime = 5;
        PhoneManage phonemange = new PhoneManage();
        BaseManagementOther _bmBaseDB = new BaseManagementOther();
        readonly BaseManagement _bm = new BaseManagement();
        public string AppId = WebConfigurationManager.AppSettings["AppID"];
        string FiedURL = WebConfigurationManager.AppSettings["FileServerUrl"];
        UserManagement usermanager = new UserManagement();
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string BaseDB = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunBaseDBConnectionStr"].ConnectionString;

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        /// <summary>
        /// 
        /// </summary>
        public object ArryList { get; private set; }

        [HttpGet]
        public void Test()
        {
            string cl = Guid.NewGuid().ToString();
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            string s1 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(cl, "MD5").ToLower();
            string s3 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s1, "MD5").ToLower();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            string s2 = md5.ComputeHash(Encoding.UTF8.GetBytes(cl)).ToString();
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            string pwd = s.Aggregate("", (current, t) => current + t.ToString("X"));
            string sss = pwd;
        }

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse QueryUserCombo(string UserID)
        {

            var temp = QueryCombo(UserID);
            if (temp == null)
            {
                GetErrorResult1("没有可用的权限信息");
            }
            return GetResult1(temp);
        }

        /// <summary>
        /// 获取用户权限信息(包含模块权限)
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse QueryUserComboWithModule(string UserID)
        {
            var temp = QueryComboWithModule(UserID);
            if (temp == null)
            {
                GetErrorResult1("没有可用的权限信息");
            }
            return GetResult1(temp);
        }

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <returns></returns>
        public ArrayList QueryCombo(string UserID)
        {

            ArrayList list = new ArrayList();
            try
            {
                #region 获取同步课堂权限
                TBService.WebServicePatch wp = new TBService.WebServicePatch();
                var tlist = wp.LoadSyncClassBind(UserID);
                if (tlist != null)
                {
                    foreach (var item in tlist)
                    {
                        list.Add(new
                        {
                            UserID = UserID,
                            CourseID = item.BookID ?? "",
                            EndDate = (DateTime.Now.AddMonths(12).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                            Months = 1
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "QueryCombo获取同步课堂权限出错，UserID=" + UserID);
            }
            finally
            {
                string sql = string.Format(@"SELECT DISTINCT
                                                    CourseID ,
                                                    SUM(Months) 'Months'
                                            FROM    dbo.TB_UserMember
                                            WHERE   UserID = '{0}'
                                            GROUP BY CourseID
                                            ORDER BY CourseID", UserID);
                DataSet ds = SqlHelper.ExecuteDataset(BaseDB, CommandType.Text, sql);
                List<memberinfo> mi = JsonHelper.DataSetToIList<memberinfo>(ds, 0);
                string strsql = string.Format(@"SELECT  [ID]
                                                      ,[UserID]
                                                      ,[TbOrderID]
                                                      ,[Months]
                                                      ,[StartDate]
                                                      ,[EndDate]
                                                      ,[CourseID]
                                                      ,[Status]
                                                      ,[CreateTime]
                                                      ,[SourceType]
                                                  FROM [TB_UserMember] WHERE UserID='" + UserID + "' and (ModuleID is null or ModuleID='' or ModuleID='1') order By EndDate desc");
                DataSet datads = SqlHelper.ExecuteDataset(BaseDB, CommandType.Text, strsql);
                List<TB_UserMember> umList = JsonHelper.DataSetToIList<TB_UserMember>(datads, 0);
                //var umList = usermanager.Search<TB_UserMember>("UserID='" + UserID + "'  order By EndDate desc");
                if (umList != null)
                {
                    if (mi != null)
                    {
                        foreach (var em in mi)
                        {
                            TB_UserMember tu = umList.Where(i => i.CourseID == em.CourseID).OrderBy(i => i.CreateTime).FirstOrDefault();
                            List<TB_UserMember> usermember = umList.Where(i => i.CourseID == em.CourseID).OrderBy(i => i.CreateTime).ToList();
                            if (tu != null)
                            {
                                if (tu.CreateTime != null)
                                {
                                    if (tu.Months != null)
                                    {
                                        int Months = (int)tu.Months * usermember.Count;

                                        list.Add(new
                                        {
                                            UserID = UserID,
                                            CourseID = tu.CourseID,
                                            EndDate = (tu.CreateTime.Value.AddMonths(Months).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                            Months = Months
                                        });
                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (list.Count == 0) return null;
            return list;
            //return "";
        }

        /// <summary>
        /// 获取用户权限信息(包含模块权限)
        /// </summary>
        /// <returns></returns>
        public ArrayList QueryComboWithModule(string UserID)
        {

            ArrayList list = new ArrayList();
            try
            {
                #region 获取同步课堂权限
                TBService.WebServicePatch wp = new TBService.WebServicePatch();
                var tlist = wp.LoadSyncClassBind(UserID);
                if (tlist != null)
                {
                    foreach (var item in tlist)
                    {
                        list.Add(new
                        {
                            UserID = UserID,
                            CourseID = item.BookID ?? "",
                            EndDate = (DateTime.Now.AddMonths(12).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                            Months = 1
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "QueryCombo获取同步课堂权限出错，UserID=" + UserID);
            }
            finally
            {
                string sql = string.Format(@"SELECT DISTINCT
                                                    CourseID ,
                                                    SUM(Months) 'Months',
                                                    ModuleID
                                            FROM    dbo.TB_UserMember
                                            WHERE   UserID = '{0}'
                                            GROUP BY CourseID,ModuleID
                                            ORDER BY CourseID", UserID);
                DataSet ds = SqlHelper.ExecuteDataset(BaseDB, CommandType.Text, sql);
                List<memberinfo> mi = JsonHelper.DataSetToIList<memberinfo>(ds, 0);
                string strsql = string.Format(@"SELECT  [ID]
                                                      ,[UserID]
                                                      ,[TbOrderID]
                                                      ,[Months]
                                                      ,[StartDate]
                                                      ,[EndDate]
                                                      ,[CourseID]
                                                      ,[Status]
                                                      ,[CreateTime]
                                                      ,[SourceType]
                                                      ,[ModuleID]
                                                  FROM [TB_UserMember] WHERE UserID='" + UserID + "'  order By EndDate desc");//and  ModuleID is not null and ModuleID!=''
                DataSet datads = SqlHelper.ExecuteDataset(BaseDB, CommandType.Text, strsql);
                List<TB_UserMember> umList = JsonHelper.DataSetToIList<TB_UserMember>(datads, 0);
                //var umList = usermanager.Search<TB_UserMember>("UserID='" + UserID + "'  order By EndDate desc");
                if (umList != null)
                {
                    if (mi != null)
                    {
                        foreach (var em in mi)
                        {
                            TB_UserMember tu = umList.Where(i => i.CourseID == em.CourseID && i.ModuleID==em.ModuleID).OrderBy(i => i.CreateTime).FirstOrDefault();
                            //List<TB_UserMember> usermember = umList.Where(i => i.CourseID == em.CourseID).OrderBy(i => i.CreateTime).ToList();
                            if (tu != null)
                            {
                                if (tu.CreateTime != null)
                                {
                                    if (tu.Months != null)
                                    {
                                        //int Months = (int)tu.Months * usermember.Count;

                                        string ModuleID;
                                        if (string.IsNullOrEmpty(em.ModuleID))
                                        {
                                            ModuleID = "1";//代表EBook
                                        }
                                        else
                                        {
                                            ModuleID = em.ModuleID;
                                        }

                                        list.Add(new
                                        {
                                            UserID = UserID,
                                            CourseID = em.CourseID,
                                            EndDate = (tu.CreateTime.Value.AddMonths(em.Months).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                            Months = em.Months,
                                            ModuleID = ModuleID
                                        });
                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (list.Count == 0) return null;
            return list;
            //return "";
        }

        private ApiResponse GetErrorResult1(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult1(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }



        /// <summary>
        /// APP用户发送手机短信
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SendShortMessages([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("电话号码错误!");
            }

            string sql = string.Format("  SELECT TOP 1 * FROM dbo.Tb_PhoneCode WHERE TelePhone='{0}'  AND EndDate>'{1}' AND State=1  ORDER BY EndDate DESC", submitData.TelePhone, DateTime.Now);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return GetErrorResult("请使用五分钟内获取的验证码登陆!");
            }
            Tb_PhoneCode phonecode = new Tb_PhoneCode();
            phonecode.Code = CommonHelper.RndNumRNG(6);
            phonecode.EndDate = DateTime.Now.AddMinutes(EndMessageCodeTime);
            phonecode.TelePhone = submitData.TelePhone.Trim();
            if (phonemange.InInsert(phonecode))
            {
                //验证码缓存起来 为5分钟有效
                SMSService.SMSService smssmessage = new SMSService.SMSService();
                string MessageContent = "您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], submitData.TelePhone, MessageContent);
                string[] resultArr = results.Split(new char[] { ',' });
                if (resultArr[0] == "0" || resultArr[0] == "200")
                {
                    var returnObj =
                        new
                        {
                            CheckCode = "",
                            TelePhone = submitData.TelePhone,
                            Success = true,
                            Msg = ""
                        };
                    return GetResult(JsonHelper.EncodeJson(returnObj));
                }
                else
                {
                    return GetErrorResult("验证码发送失败");
                }

            }
            return GetErrorResult("发送失败");
        }

        /// <summary>
        /// 判断手机和验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DecidePhoneCode([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("手机号不能为空");
            }
            if (string.IsNullOrEmpty(submitData.MessageCode))
            {
                return GetErrorResult("验证码不能为空");
            }
            if (phonemange.CheckPhoneCode(submitData.TelePhone, submitData.MessageCode))
            {
                return GetResult(request);
            }
            else
            {
                return GetErrorResult("验证码输入错误");
            }
        }
        /// <summary>
        /// 创建token 
        /// </summary>
        /// <param name="token"></param>
        [HttpPost]
        public HttpResponseMessage CreateToken([FromBody]KingRequest request)
        {
            loginModel token = JsonHelper.DecodeJson<loginModel>(request.Data);
            string tk = AddToken(token.UserID, token.EquipmentID);
            if (!string.IsNullOrEmpty(tk))
                return GetResult(tk);

            return JsonHelper.GetErrorResult(100102, "");
        }

        public string AddToken(string UserId, string EquipmentID)
        {
            string tk = pswToSecurity(UserId + "|" + EquipmentID);
            RedisHashHelper hase = new RedisHashHelper();
            string tokenname = System.Web.Configuration.WebConfigurationManager.AppSettings["tokename"];
            hase.Remove(tokenname, UserId);
            if (hase.Set(tokenname, UserId, tk + "|" + DateTime.Now.ToString()))
                return tk;
            return "";
        }


        /// <summary>
        ///  验证token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VerificationToken([FromBody]KingRequest request)
        {
            return GetResult("验证通过");
            //loginModel entitytoken = JsonHelper.DecodeJson<loginModel>(request.Data);
            //RedisHashHelper hase = new RedisHashHelper();
            //string tokenname = System.Web.Configuration.WebConfigurationManager.AppSettings["tokename"];
            //string tokenvalue = hase.Get(tokenname, entitytoken.UserID);
            //if (!string.IsNullOrEmpty(tokenvalue))
            //{
            //    string[] array = tokenvalue.Split('|');
            //    string Token = array[0];

            //    if (Token == pswToSecurity(entitytoken.UserID.Trim() + "|" + entitytoken.EquipmentID.Trim()))
            //    {
            //        int interval = new TimeSpan(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).Ticks - Convert.ToDateTime(Convert.ToDateTime(array[1]).ToString("yyyy-MM-dd")).Ticks).Days;
            //        if (interval > 30)
            //            return JsonHelper.GetErrorResult(100103, "");
            //        return GetResult("验证通过");
            //    }
            //}
            //return JsonHelper.GetErrorResult(100102, "");
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


   
        //记录使用次数 
        [HttpGet]
        [ShowApi]
        public HttpResponseMessage RecordUsageNumberTest()
        {

            UsageDetails submitData = new UsageDetails();
            submitData.AppId = "appid";
            submitData.Versions = "v1.0";
            //submitData.DownloadChannel = 2;
            submitData.UserId = "2024285613";//  22093164 1919933961
            submitData.UsageTimeLength = 0;
            submitData.UsageNumber = 1;
            try
            {
                return GetResult(AppSetting.SetValidUserUsageNumber(submitData.UserId, submitData.AppId, submitData.Versions, submitData.DownloadChannel));
            }
            catch (Exception ex)
            {
                return GetErrorResult("error:" + ex);
            }
        }

        //记录使用时长
        [HttpGet]
        [ShowApi]
        public HttpResponseMessage RecordUsageTimeLengthTest()
        {
            UsageDetails submitData = new UsageDetails();
            submitData.AppId = "appid";
            submitData.Versions = "1";
            submitData.UserId = "123456789";
            submitData.UsageTimeLength = 10;
            try
            {

                return GetResult(AppSetting.SetValidUserUsageTimeLength(submitData.UserId, submitData.AppId, submitData.Versions, submitData.UsageTimeLength, submitData.DownloadChannel));
            }
            catch (Exception ex)
            {
                return GetErrorResult("error:" + ex);
            }
        }


        //记录使用时长
        [HttpPost]
        [ShowApi]
        public HttpResponseMessage RecordUsageTimeLength([FromBody]KingRequest request)
        {
            UsageDetails submitData = JsonHelper.DecodeJson<UsageDetails>(request.Data);

            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppId))
            {
                return GetErrorResult("AppId不能为空");
            }
            if (string.IsNullOrEmpty(submitData.Versions))
            {
                return GetErrorResult("版本不能为空");
            }
            if (submitData.DownloadChannel != null && string.IsNullOrEmpty(submitData.DownloadChannel.ToString()))
            {
                return GetErrorResult("渠道不能为空");
            }
            if (submitData.UserId != null && string.IsNullOrEmpty(submitData.UserId.ToString()))
            {
                return GetErrorResult("用户ID不能为空");
            }
            if (submitData.UsageTimeLength != null && string.IsNullOrEmpty(submitData.UsageTimeLength.ToString()))
            {
                return GetErrorResult("使用时长不能为空");
            }


            #endregion
            try
            {
                return GetResult(AppSetting.SetValidUserUsageTimeLength(submitData.UserId, submitData.AppId, submitData.Versions, submitData.UsageTimeLength, submitData.DownloadChannel));
            }
            catch (Exception ex)
            {
                return GetErrorResult("error:" + ex);
            }
        }


        //手机APP登录
        [HttpPost]
        [ShowApi]
        public HttpResponseMessage AppLogin([FromBody]KingRequest request)
        {

            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserName))
            {
                return GetErrorResult("用户名不能为空");
            }
            if (string.IsNullOrEmpty(submitData.PassWord))
            {
                return GetErrorResult("密码不能为空");
            }
            //if (string.IsNullOrEmpty(submitData.MachineCode))
            //{
            //    return GetErrorResult("机器码不能为空");
            //}
            //if (string.IsNullOrEmpty(submitData.MachineModel))
            //{
            //    return GetErrorResult("机器模式不能为空");
            //}
            if (submitData.IsEnableOss == null)
            {
                submitData.IsEnableOss = 0;
            }
            #endregion

            try
            {
                TB_UserInfoExtend uie = new TB_UserInfoExtend();
                UserInfoListNum uiln = new UserInfoListNum();

                uie.EquipmentID = submitData.EquipmentID;
                uie.DeviceType = submitData.DeviceType;
                uie.IPAddress = submitData.IPAddress;
                uie.CreateDate = DateTime.Now;
                var rinfo = userBLL.AppLogin(submitData.UserName.Trim(), submitData.PassWord, submitData.MachineCode, ProjectConstant.AppID, submitData.MachineModel, uie);

                if (rinfo.Success)
                {
                    string[] strs = rinfo.Data.ToString().Split('|');
                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(strs[0]));
                    if (user != null)
                    {
                        AppSetting.SetValidUserRecord(user.UserID.ToString());

                        //获取该用户的历史信息
                        if (strs.Length > 1)
                        {
                            //获取该用户的历史信息
                            uiln.UserNum = strs[1];
                        }

                        OnlineUser oUser = new OnlineUser();
                        OnlineUserImplement implete = new OnlineUserImplement();
                        implete.RemoveOnlineUser(user.UserID.ToString());
                        if (user.ClassSchList != null)
                        {
                            var ri = user.ClassSchList.FirstOrDefault();
                            if (ri != null)
                            {
                                var cl = classBLL.GetClassUserRelationByClassId(ri.ClassID);
                                if (cl != null)
                                {
                                    uiln.ClassNum = cl.ClassNum ?? "";
                                }
                            }
                        }
                        uiln.NickName = string.IsNullOrEmpty(user.TrueName) ? "暂未填写" : user.TrueName;
                        uiln.TelePhone = user.TelePhone;
                        uiln.UserID = Convert.ToInt32(user.UserID);
                        uiln.TrueName = string.IsNullOrEmpty(user.TrueName) ? "暂未填写" : user.TrueName;
                        if (submitData.IsEnableOss == 0)
                        {
                            uiln.UserImage = user.UserImage;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(user.UserImage))
                            {
                                user.UserImage = "00000000-0000-0000-0000-000000000000";

                            }
                            uiln.UserImage = user.IsEnableOss == 0 ? _getOssFilesUrl + user.UserImage : _getFilesUrl + "?FileID=" + user.UserImage;
                        }
                        uiln.UserName = string.IsNullOrEmpty(user.TrueName) ? "暂未填写" : user.UserName;
                        uiln.UserRoles = user.UserRoles;

                        //user = implete.GetOnlineuserByService(userinfo.UserID);
                        //implete.AddOnlineUser(user);
                        uiln.ComboInfo = QueryCombo(user.UserID.ToString());
                        uiln.UserType = user.UserType;

                        if (user.UserType == (int)UserTypeEnum.Teacher)
                        {
                            if (string.IsNullOrEmpty(user.SchoolName))
                            {
                                uiln.needImproveSource = "true";
                            }
                            else
                            {
                                uiln.needImproveSource = "false";
                            }
                            uiln.SchoolID = user.SchoolID;
                            uiln.SchoolName = user.SchoolName;

                        }
                        else
                        {
                            if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                            {
                                var classinfo = user.ClassSchList.First();
                                uiln.SchoolID = classinfo.SchID;
                                IIBSData_SchClassRelationBLL schBLL=new IBSData_SchClassRelationBLL();
                                var sch = schBLL.GetSchClassRelationBySchlId(classinfo.SchID);
                                if (sch != null)
                                {
                                    uiln.SchoolName = sch.SchName;
                                }
                                else
                                {
                                    uiln.SchoolName = "";
                                }
                               
                            }
                            else
                            {
                                uiln.SchoolID = 0;
                                uiln.SchoolName = "";
                            }
                        }


                        // 登录时数据埋点

                        AppSetting.SetValidUserLogin(strs[0], ProjectConstant.AppID, submitData.Versions, submitData.DownloadChannel);


                        #region 获取用户身份信息 添加 token

                        uiln.Token = AddToken(user.UserID.ToString(), submitData.EquipmentID);
                    }
                        #endregion

                    return ObjectToJson.GetResult(uiln);
                }
                else
                {
                    return GetErrorResult("登陆失败！" + rinfo.ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex,"登录接口报错！Data="+ex.Message);
                return GetErrorResult("error:" + ex);
            }
        }

        //手机APP登录
        [HttpGet]
        [ShowApi]
        public HttpResponseMessage AppLoginTest()
        {
            loginModel submitData = new loginModel();//JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性

            submitData.UserName = "ls015";
            submitData.PassWord = "123456";
            submitData.EquipmentID = "909e0dd9ccb32fcf";
            submitData.IPAddress = "183.47.42.218";
            submitData.DeviceType = "SM-G9300";
            submitData.MachineCode = "1234455";
            submitData.MachineModel = "Android";
            submitData.IsEnableOss = 1;

            #endregion

            try
            {
                TB_UserInfoExtend uie = new TB_UserInfoExtend();
                UserInfoListNum uiln = new UserInfoListNum();
                uie.EquipmentID = submitData.EquipmentID;
                uie.DeviceType = submitData.DeviceType;
                uie.IPAddress = submitData.IPAddress;
                uie.CreateDate = DateTime.Now;
                var rinfo = userBLL.AppLogin(submitData.UserName.Trim(), submitData.PassWord, submitData.MachineCode, ProjectConstant.AppID, submitData.MachineModel, uie);

                if (rinfo.Success)
                {
                    string[] strs = rinfo.Data.ToString().Split('|');
                    var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(strs[0]));
                    if (user != null)
                    {
                        AppSetting.SetValidUserRecord(user.iBS_UserInfo.UserID.ToString());
                        if (strs.Length > 1)
                        {
                            //获取该用户的历史信息
                            uiln.UserNum = strs[1];
                        }
                       
                        OnlineUser oUser = new OnlineUser();
                        OnlineUserImplement implete = new OnlineUserImplement();
                        implete.RemoveOnlineUser(user.iBS_UserInfo.UserID.ToString());
                        if (user.ClassSchDetailList != null)
                        {
                            var ri = user.ClassSchDetailList.FirstOrDefault();
                            if (ri != null)
                            {
                                var cl = classBLL.GetClassUserRelationByClassId(ri.ClassID);
                                if (cl != null)
                                {
                                    uiln.ClassNum = cl.ClassNum ?? "";
                                }
                            }
                        }
                        uiln.NickName = user.iBS_UserInfo.TrueName;
                        uiln.TelePhone = user.iBS_UserInfo.TelePhone;
                        uiln.UserID = Convert.ToInt32(user.iBS_UserInfo.UserID);
                        uiln.TrueName = user.iBS_UserInfo.TrueName;
                        uiln.UserImage = user.iBS_UserInfo.UserImage;
                        uiln.UserName = user.iBS_UserInfo.UserName;
                        uiln.UserRoles = user.iBS_UserInfo.UserRoles;

                        //user = implete.GetOnlineuserByService(userinfo.UserID);
                        //implete.AddOnlineUser(user);
                        uiln.ComboInfo = QueryCombo(user.iBS_UserInfo.UserID.ToString());
                        uiln.UserType = user.iBS_UserInfo.UserType;

                        if (string.IsNullOrEmpty(user.iBS_UserInfo.SchoolName))
                        {
                            uiln.needImproveSource = "true";
                        }
                        else
                        {
                            uiln.needImproveSource = "false";
                        }
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            var sc = user.ClassSchDetailList.First();
                            uiln.SchoolName = sc.SchName;
                            uiln.SchoolID = sc.SchID;

                        }

                        #region 登录时数据埋点

                        submitData.Versions = "v1.0";
                      

                        AppSetting.SetValidUserLogin(rinfo.Data.ToString().Split('|')[0], ProjectConstant.AppID, submitData.Versions, submitData.DownloadChannel);

                        #endregion

                        uiln.Token = AddToken(user.iBS_UserInfo.UserID.ToString(), submitData.EquipmentID);
                    }

                    return ObjectToJson.GetResult(uiln);
                }
                else
                {
                    return GetErrorResult("登陆失败！");
                }
            }
            catch (Exception ex)
            {
                return GetErrorResult("error:" + ex);
            }
        }

        //手机APP注册
        [HttpPost]
        public HttpResponseMessage AppRegister([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            // #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserName))
            {
                return GetErrorResult("用户名不能为空");
            }
            if (string.IsNullOrEmpty(submitData.PassWord))
            {
                return GetErrorResult("密码不能为空");
            }
            if (string.IsNullOrEmpty(submitData.MessageCode))
            {
                return GetErrorResult("短信验证码不能为空");
            }
            submitData.TelePhone = submitData.UserName;
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("手机号不能为空");
            }

            bool success = usermanager.GetUserInfoByTelephone(submitData.TelePhone);
            if (!success)
            {
                return GetErrorResult("手机号已经注册。");
            }
            else
            {
                if (!phonemange.CheckPhoneCode(submitData.UserName, submitData.MessageCode))
                {
                    return GetErrorResult("验证码不正确");
                }
                var rinfo = userBLL.AppRegister2(ProjectConstant.AppID, submitData.UserName, submitData.PassWord, 1);
                if (rinfo.Success)
                {
                    if (!string.IsNullOrEmpty(rinfo.Data.ToString()))
                    {
                        //注册时数据埋点
                        AppSetting.SetValidUserRegister(rinfo.Data.ToString().Split('|')[0], ProjectConstant.AppID, submitData.Versions, submitData.DownloadChannel);
                    }
                    return GetResult(rinfo);
                }
                else
                {
                    return GetErrorResult("注册失败。" + rinfo.ErrorMsg);
                }
            }

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AppModifyPassWord([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户不能为空");
            }
            if (string.IsNullOrEmpty(submitData.PassWord))
            {
                return GetErrorResult("密码不能为空");
            }
            if (string.IsNullOrEmpty(submitData.OldPasswrod))
            {
                return GetErrorResult("旧密码不能为空");
            }
            #endregion
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.TelePhone, 1);
            if (user == null)
            {
                return GetErrorResult("找不到用户的信息");
            }
            //  PSO.UUMSService.ReturnInfo rinfo = service.AppResetPassWord(ProjectConstant.AppID, userinfo.UserID, submitData.PassWord);
            var rinfo = userBLL.AppModifyPassWord(ProjectConstant.AppID, user.iBS_UserInfo.UserID.ToString(), submitData.PassWord, submitData.OldPasswrod);
            if (rinfo.Success)
            {
                return GetResult(request);
            }
            else
            {
                return GetErrorResult("重置失败" + rinfo.ErrorMsg);
            }
        }

        /// <summary>
        /// 忘记密码重新设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AppResetPassWord([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("手机号不能为空");
            }
            if (string.IsNullOrEmpty(submitData.PassWord))
            {
                return GetErrorResult("密码不能为空");
            }
            #endregion
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.TelePhone, 1);
            if (user == null)
            {
                return GetErrorResult("找不到用户的信息");
            }
            //  PSO.UUMSService.ReturnInfo rinfo = service.AppResetPassWord(ProjectConstant.AppID, userinfo.UserID, submitData.PassWord);
            var rinfo = userBLL.AppResetPassWord(ProjectConstant.AppID, user.iBS_UserInfo.UserID.ToString(), submitData.PassWord);
            if (rinfo.Success)
            {
                return GetResult("重置成功");
            }
            else
            {
                return GetErrorResult("密码重置失败！" + rinfo.ErrorMsg);
            }

        }
        /// <summary>
        /// 忘记密码重新设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AppResetPassWordTest()
        {
            loginModel submitData = new loginModel();
            submitData.TelePhone = "18000000011";
            submitData.PassWord = "qqqq";
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("手机号不能为空");
            }
            if (string.IsNullOrEmpty(submitData.PassWord))
            {
                return GetErrorResult("密码不能为空");
            }
            #endregion
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.TelePhone, 1);
            if (user == null)
            {
                return GetErrorResult("找不到用户的信息");
            }
            //  PSO.UUMSService.ReturnInfo rinfo = service.AppResetPassWord(ProjectConstant.AppID, userinfo.UserID, submitData.PassWord);
            var rinfo = userBLL.AppResetPassWord(ProjectConstant.AppID, user.iBS_UserInfo.UserID.ToString(), submitData.PassWord);
            if (rinfo.Success)
            {
                return GetResult("重置成功");
            }
            else
            {
                return GetErrorResult("密码重置失败！" + rinfo.ErrorMsg);
            }

        }

        /// <summary>
        /// 更新用户头像昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUsers([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("ID为空");
            }
            //if (!string.IsNullOrEmpty(submitData.NickName))  //昵称不为空
            //{
            //    if (ContainsBadChar(submitData.NickName))
            //    {
            //        return GetErrorResult("昵称包含特殊字符");
            //    }
            //}
            if (string.IsNullOrEmpty(submitData.IsEnableOss.ToString()))
            {
                submitData.IsEnableOss = 0;
            }

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));//Select<Tb_UserInfo>(userInfo.UserID);
            if (user == null)
            {
                return GetErrorResult("ID没有当前用户");
            }
            else
            {
                if (!string.IsNullOrEmpty(submitData.UserImage))
                {
                    user.UserImage = submitData.UserImage;
                    //string field = FiedURL + "ConfirmHandler.ashx";
                    ////get请求
                    //string dd = JsonHelper.EncodeJson(submitData.UserImage);
                    //string values = "t=[" + dd + "]";
                    //HttpGet(field, values);
                    user.IsEnableOss = (int)submitData.IsEnableOss;
                }

                user.UserID = user.UserID;


                if (submitData.NickName != null) user.TrueName = submitData.NickName ?? "暂未填写";
                TBX_UserInfo dbUser = new TBX_UserInfo();
                dbUser.iBS_UserInfo = user;
                if (userBLL.Update2(dbUser))
                {
                    if (submitData.IsEnableOss == 1)
                    {
                        user.UserImage = _getOssFilesUrl + user.UserImage;
                    }
                    else
                    {
                        user.UserImage = user.UserImage;
                    }

                    return GetResult(user);
                }
                else
                {
                    return GetErrorResult("更新头像失败");
                }
            }
        }

        /// <summary>
        /// 更新用户头像昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateUsersTest()
        {
            loginModel submitData = new loginModel(); //JsonHelper.DecodeJson<Tb_UserInfo>(request.Data);
            //if (submitData == null)
            //{
            //    return GetErrorResult("当前信息为空");
            //}
            //if (string.IsNullOrEmpty(submitData.UserID.ToString()))
            //{
            //    return GetErrorResult("ID为空");
            //}
            //if (!string.IsNullOrEmpty(submitData.NickName))  //昵称不为空
            //{
            //    if (ContainsBadChar(submitData.NickName))
            //    {
            //        return GetErrorResult("昵称包含特殊字符");
            //    }
            //}
            //{"UserID":"1452629588","UserImage":"2017/10/12/3f29b425-212b-491b-a092-42a4ef3277e0.png","IsEnableOss":1,"TelePhone":"13260549586"}
            submitData.UserID = "1452629588";
            submitData.UserImage = "2017/10/12/3f29b425-212b-491b-a092-42a4ef3277e0.png";
            submitData.NickName = "开水";
            submitData.TelePhone = "13260549586";
            submitData.IsEnableOss = 1;



            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("ID为空");
            }
            if (!string.IsNullOrEmpty(submitData.NickName))  //昵称不为空
            {
                if (ContainsBadChar(submitData.NickName))
                {
                    return GetErrorResult("昵称包含特殊字符");
                }
            }
            if (string.IsNullOrEmpty(submitData.IsEnableOss.ToString()))
            {
                submitData.IsEnableOss = 0;
            }

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));//Select<Tb_UserInfo>(userInfo.UserID);
            if (user == null)
            {
                return GetErrorResult("ID没有当前用户");
            }
            else
            {
                if (!string.IsNullOrEmpty(submitData.UserImage))
                {
                    user.UserImage = submitData.UserImage;
                    //string field = FiedURL + "ConfirmHandler.ashx";
                    ////get请求
                    //string dd = JsonHelper.EncodeJson(submitData.UserImage);
                    //string values = "t=[" + dd + "]";
                    //HttpGet(field, values);
                }

                user.UserID = user.UserID;
                user.IsEnableOss = (int)submitData.IsEnableOss;

                if (submitData.NickName != null) user.TrueName = submitData.NickName ?? "";
                TBX_UserInfo dbUser = new TBX_UserInfo();
                dbUser.iBS_UserInfo = user;

                if (userBLL.Update(dbUser))
                {
                    if (submitData.IsEnableOss == 1)
                    {
                        user.UserImage = _getOssFilesUrl + submitData.UserImage;
                    }
                    else
                    {
                        user.UserImage = submitData.UserImage;
                    }

                    return GetResult(user);
                }
                else
                {
                    return GetErrorResult("更新头像失败");
                }
            }
        }

        /// <summary>
        /// 判断是否包含字符串 如果为空 或者包含 返回true
        /// </summary>
        /// <param name="p_StringName"></param>
        /// <returns></returns>
        public bool ContainsBadChar(string p_StringName)
        {        //如果字符串为NULLor空则返回空字符串  
            bool success = false;
            if (p_StringName.Length > 6 && p_StringName.Length < 2)
            {
                return true;
            }

            if (string.IsNullOrEmpty(p_StringName))
            {
                return true;
            }

            string _StringBadChar, _TempChar;
            string[] _ArraryBadChar;
            //_StringBadChar = "@,*,#,$,!,+,',=,--,%,^,&,?,(,), <,>,[,],{,},/,\\,;,:,\",\"\",¥,「」,、";
            _StringBadChar = "／,：,；,（,）,¥,「,」,＂,、,[,],{,},#,%,-,*,+,=,\\,|,~,＜,＞,$,€,"
                + "^,•,',#,$,%,^,&,*,(,),+,',\" ,/,;,',{,},+,=,-,`,（,）,……,%,￥,#,！,~,《,》,？,，,。,、,；,‘,【,】,+,—,—,";

            _ArraryBadChar = _StringBadChar.Split(',');
            _TempChar = p_StringName;

            for (int i = 0; i < _ArraryBadChar.Length; i++)
            {
                if (_ArraryBadChar[i].Length > 0)
                {
                    //_TempChar = _TempChar.Replace(_ArraryBadChar[i], "");
                    success = _TempChar.Contains(_ArraryBadChar[i]);
                    if (success)
                    {
                        return success;
                    }
                }
            }
            return success;

        }



        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AppLoginOut([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            //if (string.IsNullOrEmpty(submitData.UserNum))
            //{
            //    return GetErrorResult("用户登录码不能为空");
            //}
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }
            string userNum = "";
            if (!string.IsNullOrEmpty(submitData.UserNum))
            {
                userNum = submitData.UserNum;
            }
            #endregion
            var rinfo = userBLL.AppLoginOut(ProjectConstant.AppID, userNum, submitData.UserID);
            if (rinfo.Success)
            {
                return GetResult(request);
            }
            else
            {
                return GetErrorResult("注销失败。" + rinfo.ErrorMsg);
            }
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ModifyPhoneCode([FromBody]KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }
            if (string.IsNullOrEmpty(submitData.TelePhone))
            {
                return GetErrorResult("用户手机不能为空");
            }
            #endregion
            var rinfo = userBLL.VerifyUserPhone(submitData.TelePhone, submitData.UserID);
            if (rinfo.Success)
            {
                return GetResult(rinfo);
            }
            else
            {
                return GetErrorResult("修改失败。" + rinfo.ErrorMsg);
            }

        }

        /// <summary>
        /// 检查用户登录状态通过USerID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AppCheckUserState(KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }
            #endregion
            OnlineUserImplement implete = new OnlineUserImplement();
            OnlineUser user = implete.GetOnlineUser(submitData.UserID);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(submitData.UserNum))
                {
                    if (user.UserNum == submitData.UserNum)
                    {
                        return GetResult(user);
                    }
                    else
                    {
                        return GetErrorResult("用户在其他地方登陆");
                    }
                }
                return GetResult(user);
            }
            else
            {
                return GetErrorResult(implete._ErrorMsg);
            }
        }

        /// <summary>
        /// 判断手机号是否已经注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CheckPhone(KingRequest request)
        {
            loginModel submitData = JsonHelper.DecodeJson<loginModel>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.TelePhone, 1);
            if (user != null)
            {
                return GetErrorResult("手机号已经注册。");
            }
            else
            {
                return GetResult("");
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="dd"></param>
        /// <param name="ddd"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDecidePhoneCode()
        {
            loginModel submitData = new loginModel();// JsonHelper.DecodeJson<Tb_UserInfo>(request.data);
            submitData.UserID = "225889967";
            submitData.NickName = "ddd";

            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID.ToString()))
            {
                return GetErrorResult("ID为空");
            }
            if (!string.IsNullOrEmpty(submitData.NickName))  //昵称不为空
            {
                if (ContainsBadChar(submitData.NickName))
                {
                    return GetErrorResult("昵称包含特殊字符");
                }
            }
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
            if (user == null)
            {
                return GetErrorResult("ID没有当前用户");
            }
            else
            {
                //  Guid g = (Guid)submitData.UserImage;

                if (!string.IsNullOrEmpty(submitData.UserImage))
                {
                    user.UserImage = submitData.UserImage;
                    string field = FiedURL + "ConfirmHandler.ashx";
                    //get请求
                    string dds = JsonHelper.EncodeJson(submitData.UserImage);
                    string values = "t=[" + dds + "]";
                    HttpGet(field, values);
                }

                if (submitData.NickName != null) user.TrueName = submitData.NickName;
                TBX_UserInfo dbUser = new TBX_UserInfo();
                dbUser.iBS_UserInfo = user;

                if (userBLL.Update(dbUser))
                {
                    return GetResult(user);
                }
                else
                {
                    return GetErrorResult("更新头像失败");
                }
            }
        }



        /// <summary>
        /// 设置ebook免费内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetEbookConfig([FromBody] KingRequest request)
        {
            //Type:0表示免费使用目录，1表示免费使用页码
            //FirstUnit:一级目录，SecondUnit:二级目录（0：表示开放一级目录下所有二级目录），PageNumber：页码
            string FirstUnit = WebConfigurationManager.AppSettings["FirstUnit"];
            string SecondUnit = WebConfigurationManager.AppSettings["SecondUnit"];
            string PageNumber = WebConfigurationManager.AppSettings["PageNumber"];
            string Type = WebConfigurationManager.AppSettings["Type"];
            var obj = new { FirstUnit = FirstUnit, SecondUnit = SecondUnit, PageNumber = PageNumber, Type = Type };
            return ObjectToJson.GetResult(obj);
        }

        /// <summary>
        /// 获取随机2位字母+四位随机数
        /// </summary>
        /// <returns></returns>
        public string NickName()
        {
            string[] s1 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            Random rand = new Random();

            return "用户" + s1[rand.Next(0, s1.Length)] + s1[rand.Next(0, s1.Length)] + rand.Next(0, 9999);
        }

        private HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, data = "", Message = message };
            return KingsunResponse.toJson(obj);
        }

        private HttpResponseMessage GetResult(object Data, string message = "")
        {
            object obj = new { Success = true, data = Data, Message = message };
            return KingsunResponse.toJson(obj);
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        //班级ID信息
        [Serializable]
        public class ReceiveInfo
        {
            public string ClassID { get; set; }

            public static ReceiveInfo DecodeRequest(string json)
            {
                ReceiveInfo request = null;
                try
                {
                    request = JsonHelper.DecodeJson<ReceiveInfo>(json);
                }
                catch
                {
                }
                return request;
            }
        }

        //返回学生信息
        private class StuInfo
        {
            public string StudentID { get; set; }
        }

        //返回学生信息
        private class StatisticsInfo
        {
            public int UserID { get; set; }
            public int Count { get; set; }
        }


        private class ActiveInfo
        {
            public string ClassID { get; set; }
            public int UserCount { get; set; }
        }

    }

    public class memberinfo
    {
        public string CourseID { get; set; }
        public int Months { get; set; }

        public string ModuleID { get; set; }
    }

    public class GetJsonList
    {
        public string UserID { get; set; }
        public string NickName { get; set; }
        public string UserImage { get; set; }
    }
    public class Token
    {
        public string UserID { get; set; }
        public string EquipmentID { get; set; }
    }

    public class SchoolInfo
    {
        public int ID { get; set; }
        public string SchoolName { get; set; }
        public int DistrictID { get; set; }
    }

}
