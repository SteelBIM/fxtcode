using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.Web.ShareApp.Handler
{
    /// <summary>
    /// ShareHandler 的摘要说明
    /// </summary>
    public class ShareHandler : IHttpHandler
    {

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        PhoneManage phonemange = new PhoneManage();
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string queryKey = context.Request["queryKey"].ToLower() ?? "";
            switch (queryKey)
            {
                case "sendcode":
                    SendCode(context);
                    break;
                case "loginbyphone":
                    LoginByPhone(context);
                    break;
                case "getusername":
                    GetUserName(context);
                    break;
                default:
                    context.Response.Write("{\"Result\":\"false\",\"msg\":\"\",\"data\":\"\"}");
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 向手机发送验证码
        /// </summary>
        /// <param name="context"></param>
        private void SendCode(HttpContext context)
        {
            int EndMessageCodeTime = 5;
            string telephone = context.Request.Form["Telephone"].ToString();
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
                Tb_PhoneCode phonecode = new Tb_PhoneCode();
                phonecode.Code = CommonHelper.RndNumRNG(6);
                phonecode.EndDate = DateTime.Now.AddMinutes(EndMessageCodeTime);
                phonecode.TelePhone = telephone.Trim();
                if (phonemange.InInsert(phonecode))
                {
                    //string checkCode = Utils.Number(6);
                    //验证码缓存起来 为5分钟有效
                    if (context.Cache[telephone] != null)
                    {
                        context.Cache.Remove(telephone);
                    }

                    context.Cache.Insert(telephone, telephone + "," + phonecode.Code, null, DateTime.Now.AddMinutes(5),
                        System.Web.Caching.Cache.NoSlidingExpiration); //这里给数据加缓存，设置缓存时间
                    SMSService.SMSService smssmessage = new SMSService.SMSService();
                    string MessageContent = "您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                    string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, MessageContent);
                    //string MessageContent = "{\"code\":\" " + phonecode.Code + "\"}";
                    //string smskye = AppSetting.GetValue("SMSKey");
                    //string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, MessageContent, smskye);

                    string[] resultArr = results.Split(new char[] { ',' });
                    if (resultArr[0] == "0" || resultArr[0] == "200")
                    {
                        var returnObj = new { CheckCode = "", Success = true, Msg = "" };
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
            context.Response.End();
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="context"></param>
        private void LoginByPhone(HttpContext context)
        {
            string telephone = context.Request.Form["Telephone"].ToString();
            string Code = context.Request.Form["Code"].ToString();
            if (string.IsNullOrEmpty(Code))
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
            if (phonemange.CheckPhoneCode(telephone, Code))
            {
                var returnInfo=userBLL.TBXLoginByPhone(AppID, telephone, 26);
                if (returnInfo.Data == null || returnInfo.Data == "")
                {
                    var obj = new { Success = false, Msg = "注册失败!" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                else
                {
                    string userID = "";
                    string userName = "";
                    if (returnInfo.Data.ToString().Split('|').Length > 1)
                    {
                        userID = returnInfo.Data.ToString().Split('|')[0];
                        userName = returnInfo.Data.ToString().Split('|')[1];
                    }
                    else
                    {
                        userID = returnInfo.Data.ToString().Split('|')[0];
                    }

                    var user = userBLL.Search(a=>a.UserID==Convert.ToInt32(userID));
                    //PSO.UUMSService.User info = UUMSService.GetUserInfoByID(AppID, userID);
                    if (user == null)
                    {
                        user = new Tb_UserInfo();
                        user.UserID = string.IsNullOrEmpty(userID.ToString()) ? 0 : Convert.ToInt32(userID.ToString());
                        user.UserName = userName;
                        user.TelePhone = telephone;
                        user.TrueName = NickName();
                        user.IsUser = 1;
                        user.IsEnableOss = 0;
                        user.isLogState = "0";
                        userBLL.Insert(user);
                        string sql = string.Format("INSERT  INTO dbo.tb_ShareAppUser( UserID )VALUES  ( '{0}' )", userID);
                        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                    }

                    if (returnInfo.Data != null)
                    {
                        var obj = new { Success = true, UserID = userID };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = false, UserID = userID };
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
        /// 获取用户昵称
        /// </summary>
        /// <param name="context"></param>
        private void GetUserName(HttpContext context)
        {
            string userID = context.Request.Form["UserID"].ToString();

            if (string.IsNullOrEmpty(userID) || userID == "undefined")
            {
                var returnObj = new { Success = false, Msg = "用户ID不能为空!" };
                context.Response.Write(JsonHelper.EncodeJson(returnObj));
                context.Response.End();
            }

            string userName = "";
            string img = "";
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userID));
            if (user != null) 
            {
                img = user.IsEnableOss != 0 ? _getOssFilesUrl + user.UserImage : _getFilesUrl + "?FileID=" + user.UserImage;
                userName = user.TrueName;
            }
           
            var obj = new { Success = true, userName = userName, UserImg = img };
            context.Response.Write(JsonHelper.EncodeJson(obj));
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
    }
}