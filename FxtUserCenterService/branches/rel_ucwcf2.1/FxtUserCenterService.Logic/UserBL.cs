using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using FxtUserCenterService.DataAccess;
using CAS.Common;
using System.Data;
using CAS.Entity;
using System.Text.RegularExpressions;
using System.IO;
namespace FxtUserCenterService.Logic
{
    public class UserBL
    {
        public static int Update(FxtUserCenterService.Entity.UserInfo model)
        {
            return UserDA.Update(model);
        }

        public static int Add(FxtUserCenterService.Entity.UserInfo model)
        {
            return UserDA.Add(model);
        }

        public static int Delete(string username, int companyid,int valid)
        {
            return UserDA.Delete(username, companyid,valid);
        }

        public static UserCheck GetCheckUser(string username, int systypecode)
        {
            return UserDA.GetCheckUser(username, systypecode);
        }

        /// <summary>
        /// 检查用户 caoq 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserCheck GetCheckUser(string username)
        {
            return UserDA.GetCheckUser(username);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static UserCheck GetFindUser(string username)
        {
            return UserDA.GetFindUser(username);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public static int EditUser(FxtUserCenterService.Entity.UserInfo userinfo, string password)
        {
            return UserDA.EditUser(userinfo, password);
        }

        /// <summary>
        /// 获取用户列表 caoq 2014-03-04
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyid">公司ID</param>
        /// <param name="companycode">公司CODE</param>
        /// <param name="valid">是否可用,1:可用,0:不可用,null:所有</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="userids">用户ID列表</param>
        /// <returns></returns>
        public static List<UserCheck> GetUserList(SearchBase search, int companyid, string companycode, int? valid, string keyword, string[] userids)
        {
            return UserDA.GetUserList(search, companyid, companycode,valid,keyword, userids);
        }

        /// <summary>
        /// 获取用户信息与安全信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="productTypeCode">产品Code</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static WCFJsonData GetUserAndAppInfo(SearchBase search, string pwd, int productTypeCode, string userName, string appabbreviation, string weburl, string splatype = null, string channel = null)
        {
            WCFJsonData jsondata = new WCFJsonData();

            InheritUserInfo inheri = UserDA.GetUserAndAppInfo(search, productTypeCode,userName, appabbreviation,weburl);

            

            if (inheri == null)
            {
#if DEBUG
                jsondata = JSONHelper.GetWcfJson((int)EnumHelper.Status.LoginFailure, "用户不存在");
#else
                jsondata = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
#endif

            }
            else
            {

                //验证产品有效 kevin 20140408
                if (DateTime.Now > inheri.overdate)
                {
                    jsondata = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "产品已超过有效期");
                }
                else
                {
                    string password = EncryptHelper.GetMd5(EncryptHelper.PasswordToText(inheri.userpwd) + ConstCommon.WcfPassWordMd5Key);

                    if (pwd != password)
                    {

#if DEBUG
                    jsondata = JSONHelper.GetWcfJson((int)EnumHelper.Status.LoginFailure, "密码错误");
#else
                        jsondata = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
#endif

                        //jsondata = JSONHelper.GetWcfJson((int)EnumHelper.Status.LoginFailure, "密码错误");
                    }
                    else
                    {
                        string md = EncryptHelper.GetMd5(userName + ConstCommon.WcfUserNameMd5Key);//账号md5加密
                        string d = EncryptHelper.TextToPassword(userName);//账号desc加密
                        string um = inheri.userpwd;
                        //验证是否为简单密码

                        //如果账号为gjb或demo，就不检测账号是否为简单账号
                        if (!(userName.IndexOf("@gjb") > -1 || userName.IndexOf("@demo") > -1))
                        {
                            string oldpwd = EncryptHelper.PasswordToText(inheri.userpwd);
                            password = (oldpwd + ConstCommon.WcfPassWordMd5Key);
                            string[] ignore = WebCommon.GetConfigSetting("pwdsimpleischecksystypecode").Split(',');
                            string pattern = @"[a-zA-Z]+\S+[0-9]+|[0-9]+[a-zA-Z]+";
                            if (ignore.Length > 0)
                            {
                                for (int i = 0; i < ignore.Length; i++)
                                {
                                    if (ignore[i] == productTypeCode.ToString())
                                    {
                                        if (oldpwd.Length < 6 || oldpwd.Length > 20 || !Regex.IsMatch(oldpwd, pattern) || SimplePassWordBL.CheckIsSimplePassWord(oldpwd) ==1)
                                        {
                                            string token = WebCommon.GetRndString(20);
                                            FileHelper.SaveToken(token, string.Format("{0},{1}", userName, inheri.userpwd));
                                            jsondata = JSONHelper.GetWcfJson(string.Format("{1}default.aspx?&token={0}", token, WebCommon.GetConfigSetting("updatepwdurl")), (int)EnumHelper.Status.SimplePassWord, ConstCommon.WcfSimpleTip);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        var apps = UserDA.GetApps(search, productTypeCode, userName, appabbreviation);
                        UserAndAppInfo userInfo = new UserAndAppInfo();
                        Security secur = new Security();
                        secur.signname = inheri.signname;
                        secur.producttypecode = inheri.producttypecode;
                        secur.weburl = inheri.weburl;
                        secur.businessdb = inheri.businessdb;

                        if (new string[] { "ios", "android" }.Contains(splatype))
                        {
                            var token = GenerateToken(userName, splatype);
                            MobilePushDA.LoginSaveToken(userName, channel, productTypeCode, token);
                            secur.token = token;

                        }
                        else
                        {
                            secur.token = "";
                        }

                        var appsList = new List<FxtUserCenterService.Entity.Apps>();

                        apps.ForEach(m => appsList.Add(new FxtUserCenterService.Entity.Apps
                        {
                            appid = m.appid,
                            apppwd = m.apppwd,
                            appurl = m.apiurl,
                            appkey = m.appkey

                        }));

                        secur.apps = appsList;

                        userInfo.sinfo = secur;
                        CUserInfo cuInfo = new CUserInfo();
                        cuInfo.truename = inheri.truename;
                        cuInfo.fxtcompanyid = inheri.companyid;
                        cuInfo.companyname = inheri.companyname;
                        cuInfo.subcompanyid = "";
                        cuInfo.subcompany = "";
                        cuInfo.editpwdurl = jsondata.data;
                        cuInfo.returntext = jsondata.returntext;
                        cuInfo.status = jsondata.returntype;
                        userInfo.uinfo = cuInfo;
                        jsondata = JSONHelper.GetWcfJson(userInfo, (int)EnumHelper.Status.Success, "Success");
                    }
                }
            }
            return jsondata;
        }

        /// <summary>
        /// 产生token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="splatype"></param>
        /// <returns></returns>
        private static string GenerateToken(string username, string splatype)
        {
            string str = username + splatype + Guid.NewGuid().ToString() + ConstCommon.WcfMobileTokenKey;

            return EncryptHelper.GetMd5(str);
        }

        /// <summary>
        /// modify: 获取用户密码 hody 2014-04-08
        /// 验证用户密码是否正确 caoq 2014-4-4
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetUserPassword(string username)
        {
            return UserDA.GetUserPassword(username);
        }

        /// <summary>
        /// 根据用户名获取用户信息 caoq 2014-06-10
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static FxtUserCenterService.Entity.UserInfo GetUserInfoByUserName(string username)
        {
            return UserDA.GetUserInfoByUserName(username);
        }


        /// <summary>
        /// 修改用户密码 hody 2014-07-25
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int UpdatePassWord(string username, string userpwd, string oldpwd)
        {
            return UserDA.UpdatePassWord(username, userpwd, oldpwd);
        }
    }

}
