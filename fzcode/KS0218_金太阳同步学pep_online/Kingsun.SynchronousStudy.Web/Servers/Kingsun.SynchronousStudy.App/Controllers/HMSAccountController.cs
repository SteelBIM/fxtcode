using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System.Web.Configuration;
using System.Data;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 账号相关接口
    /// </summary>
    public class HMSAccountController : ApiController
    {
        int EndMessageCodeTime = 5;
        private readonly string _appId = WebConfigurationManager.AppSettings["AppID"];
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        PhoneManage phonemange = new PhoneManage();


        [HttpPost]
        public HttpResponseMessage CheckHwHasLogin([FromBody] KingRequest request)
        {
            HWLogin submitData = JsonHelper.DecodeJson<HWLogin>(request.Data);
            if (string.IsNullOrEmpty(submitData.openId))
            {
                return JsonHelper.GetErrorResult("OpenID不能为空！");
            }
            string sql = string.Format(@"SELECT id,Phone,OpenId,CreateTime FROM dbo.TB_HWLoginInfo WHERE OpenId='{0}'", submitData.openId);
            DataSet ds = JsonHelper.SelectOrderSql(submitData.AppId, sql);
            if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return JsonHelper.GetErrorResult("该手机未绑定");
            }
            else
            {
                string strsql = string.Format(@"SELECT Phone FROM dbo.TB_HWLoginInfo WHERE OpenId='{0}'", submitData.openId);
                object i = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strsql);
                if (!string.IsNullOrEmpty(i.ToString()))
                {
                    Tb_PhoneCode phonecode = new Tb_PhoneCode();
                    phonecode.Code = CommonHelper.RndNumRNG(6);
                    phonecode.EndDate = DateTime.Now.AddMinutes(EndMessageCodeTime);
                    phonecode.TelePhone = i.ToString().Trim();
                    if (phonemange.InInsert(phonecode))
                    {
                        var returnObj =
                            new
                            {
                                MessageCode = phonecode.Code,
                                Phone = i,
                            };
                        return JsonHelper.GetResult(JsonHelper.EncodeJson(returnObj));

                    }
                }
            }
            return null;
        }

        [HttpPost]
        public HttpResponseMessage HwOpenIdLogin([FromBody] KingRequest request)
        {
            HWLogin submitData = JsonHelper.DecodeJson<HWLogin>(request.Data);
            TB_UserInfoExtend uie = new TB_UserInfoExtend();
            UserInfoListNum uiln = new UserInfoListNum();
            if (string.IsNullOrEmpty(submitData.openId))
            {
                return JsonHelper.GetErrorResult("OpenID不能为空！");
            }

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
                            string strsql = string.Format(@"SELECT COUNT(1) FROM dbo.TB_HWLoginInfo WHERE OpenId='{0}'", submitData.openId);
                            object i = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strsql);
                            if (Convert.ToInt32(i) <= 0)
                            {
                                string sql = string.Format(@"INSERT  INTO dbo.TB_HWLoginInfo  ( Phone, OpenId, CreateTime )VALUES  ( '{0}', '{1}', GETDATE() )", submitData.Phone, submitData.openId);
                                int j = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                                if (j <= 0)
                                {
                                    Log4Net.LogHelper.Error("TB_HWLoginInfo数据插入失败，数据:" + submitData.Phone + ";OpenId=" + submitData.openId);
                                    return JsonHelper.GetErrorResult("登录失败！");
                                }
                            }
                            else
                            {
                                string sql = string.Format(@"UPDATE dbo.TB_HWLoginInfo SET Phone='{0}' WHERE OpenId='{1}'", submitData.Phone, submitData.openId);
                                int j = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                                if (j <= 0)
                                {
                                    Log4Net.LogHelper.Error("TB_HWLoginInfo数据绑定失败，数据:" + submitData.Phone + ";OpenId=" + submitData.openId);
                                    return JsonHelper.GetErrorResult("登录失败！");
                                }
                            }

                            string[] strs = re.Data.ToString().Split('|');
                            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(strs[0]));
                            if (user != null)
                            {
                                AppSetting.SetValidUserRecord(string.IsNullOrEmpty(strs[0]) ? "0" : strs[0]);
                                string img = "";

                                if (!string.IsNullOrEmpty(user.iBS_UserInfo.UserImage))
                                {
                                    img = user.iBS_UserInfo.IsEnableOss == 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                                }
                                else
                                {
                                    img = _getOssFilesUrl + submitData.photoUrl;
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
                                uiln.ClassNum = strs[2];
                                uiln.TelePhone = submitData.Phone;
                                uiln.UserNum = "";
                                uiln.NickName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? (string.IsNullOrEmpty(submitData.displayName) ? "暂未填写" : submitData.displayName) : user.iBS_UserInfo.TrueName;
                                uiln.UserImage = img;
                                uiln.UserRoles = Convert.ToInt32(user.iBS_UserInfo.UserRoles);
                                uiln.TrueName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? (string.IsNullOrEmpty(submitData.displayName) ? "暂未填写" : submitData.displayName) : user.iBS_UserInfo.TrueName;
                                uiln.ComboInfo = new AccountController().QueryCombo(uiln.UserID.ToString());
                                AccountController account = new AccountController();
                                uiln.Token = account.AddToken(uiln.UserID.ToString(), submitData.EquipmentID);
                                return ObjectToJson.GetResult(uiln);

                            }

                        }
                        return ObjectToJson.GetErrorResult("登陆失败！" + re.ErrorMsg);
                    }
                    return ObjectToJson.GetErrorResult("验证码错误！");
                }
                return ObjectToJson.GetErrorResult("手机号为空！");
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "error");
                return ObjectToJson.GetErrorResult("登陆失败" + ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UnBoundHwAccount([FromBody] KingRequest request)
        {
            UnBoundHwAccount submitData = JsonHelper.DecodeJson<UnBoundHwAccount>(request.Data);
            if (string.IsNullOrEmpty(submitData.hwOpenId))
            {
                return JsonHelper.GetErrorResult("OpenID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.hwBoundPhone))
            {
                return JsonHelper.GetErrorResult("OpenID不能为空！");
            }

            string sql = string.Format(@"DELETE dbo.TB_HWLoginInfo WHERE OpenId='{0}' AND Phone='{1}'", submitData.hwOpenId, submitData.hwBoundPhone);

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (i > 0)
            {
                return JsonHelper.GetResult("解绑成功！");
            }
            else
            {
                return JsonHelper.GetErrorResult("解绑失败！");
            }
        }


    }

    public class UnBoundHwAccount
    {
        public string hwOpenId { get; set; }
        public string hwBoundPhone { get; set; }
    }

    public class HWLogin
    {
        public string Phone { get; set; }
        public string MessageCode { get; set; }
        public string openId { get; set; }
        public string displayName { get; set; }
        public string photoUrl { get; set; }
        public string AppId { get; set; }
        public string EquipmentID { get; set; }
        public string DeviceType { get; set; }
        public string IPAddress { get; set; }
    }
}
