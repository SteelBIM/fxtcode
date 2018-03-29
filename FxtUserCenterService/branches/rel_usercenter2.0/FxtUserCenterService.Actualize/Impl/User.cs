using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {

        //对外方法名：userone 参数名：username ,systypecode
        public WCFJsonData UserCheck(string sinfo, string info)
        {
            var appInfo = JObject.Parse(info)["appinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            var username = uInfo["username"].ToString();
            var systypecode = StringHelper.TryGetInt(appInfo["systypecode"].ToString());

            WCFJsonData result = null;

            UserCheck chkuser = null;
            if (systypecode > 0)
            {
                chkuser = UserBL.GetCheckUser(username, systypecode);
            }
            else
            {
                chkuser = UserBL.GetCheckUser(username);
            }
            if (chkuser != null)
            {
                if (chkuser.uservalid == 1)
                {
                    if (chkuser.companyvalid == 1)
                    {
                        if (systypecode > 0)
                        {
                            //验证产品
                            if (chkuser.productvalid == 1)
                            {
                                if (DateTime.Now <= chkuser.overdate)
                                {
                                    result= JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "成功");
                                }
                                else
                                {
                                    result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "产品已超过有效期");
                                }
                            }
                            else
                            {
                                result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "产品无效");
                               
                            }
                        }
                        else
                        {
                            //获取公司开通的云查勘产品
                            List<CompanyProduct> prolist = CompanyProductBL.GetList(chkuser.companyid
                                , new int[] { (int)EnumHelper.Codes.SysTypeCodeSurveyEnt//云查勘企业版
                                    , (int)EnumHelper.Codes.SysTypeCodeSurveyPerson//云查勘个人版
                                    , (int)EnumHelper.Codes.SysTypeCodeSurvey_Bank //云查勘银行版
                                    }, null, 0);

                            if (prolist == null || prolist.Count() <= 0)
                            {
                                result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "产品无效");
                            }
                            else if (prolist.Where(o => o.overdate > DateTime.Now).Count() <= 0)
                            {
                                result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "产品已超过有效期");
                            }
                            else
                            {
                                prolist = prolist.Where(o => o.overdate >= DateTime.Now).ToList();//获取未过期的产品
                                if (prolist.Where(o => o.producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurveyEnt).Count() > 0)
                                {
                                    //公司已开通云查勘企业版
                                    systypecode = (int)EnumHelper.Codes.SysTypeCodeSurveyEnt;
                                }
                                else
                                {
                                    //其他产品（银行版、个人版）
                                    systypecode = prolist.FirstOrDefault().producttypecode;
                                }

                                chkuser = UserBL.GetCheckUser(username, systypecode);
                                chkuser.producttypecode = systypecode;
                                result = JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "成功");
                            }
                        }
                    }
                    else
                    {
                        result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "机构无效");
                    }
                }
                else
                {
                    result =JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "用户名无效");
                }
            }
            else
            {
                result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "用户名不存在");
            }

            return result;
        }

        //对外方法名：usertwo 参数名：username,companyid,password,emailstr,mobile,wxopenid
        public WCFJsonData UserAdd(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func["username"].ToString();
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string password = func["password"].ToString();
            string emailstr = func["emailstr"].ToString();
            string mobile = func["mobile"].ToString();
            string wxopenid = func["wxopenid"].ToString();

            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
            user.companyid = companyid;
            user.username = username;
            user.userpwd = password;
            user.emailstr = emailstr;
            user.mobile = mobile;
            user.wxopenid = wxopenid;
            int add = UserBL.Add(user);
            if (add > 0) return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "新增用户成功");
            else return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增用户失败");

        }

        //对外方法名：userthree 参数名：username
        public WCFJsonData UserFind(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = uInfo["username"].ToString();
        
            UserCheck chkuser = UserBL.GetFindUser(username);
            if (chkuser != null)
            {
                if (chkuser.uservalid == 1)
                {
                    if (chkuser.companyvalid == 1)
                    {
                        return JSONHelper.GetWcfJson(chkuser,(int)EnumHelper.Status.Success, "成功");
                    }
                    else { return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "机构无效"); }
                }
                else {return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "用户名无效"); }
            }
            else { return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "用户名不存在"); }

        }

        //对外方法名：userfour 参数名：username,companyid,password,emailstr,mobile,wxopenid,truename
        public WCFJsonData UserHandlerAdd(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func["username"].ToString();
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string password = func["password"].ToString();
            string emailstr = func["emailstr"].ToString();
            string mobile = func["mobile"].ToString();
            string wxopenid = func["wxopenid"].ToString();
            string truename = func["truename"].ToString();

            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();

            user.companyid = companyid;
            user.username = username;
            user.userpwd = password;
            user.emailstr = emailstr;
            user.mobile = mobile;
            user.wxopenid = wxopenid;
            user.truename = truename;
            int add = UserBL.Add(user);
            if (add > 0) return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "新增用户成功");
            else return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增用户失败");

        }

        //对外方法名：userfive 参数名：username,companyid,emailstr,mobile,wxopenid,truename
        public WCFJsonData UserHandlerEdit(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func["username"].ToString();
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string emailstr = func["emailstr"].ToString();
            string mobile = func["mobile"].ToString();
            string wxopenid = func["wxopenid"].ToString();
            string truename = func["truename"].ToString();
            //传递密码后，需要修改用户密码
            string password = func["password"].ToString();

            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();

            UserCheck chkuser = UserBL.GetFindUser(username);
            if (chkuser == null)  return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户名不存在");
            else
                user.companyid = companyid;
            user.username = username;
            //user.userpwd = userpwd;
            user.emailstr = emailstr;
            user.mobile = mobile;
            user.wxopenid = wxopenid;
            user.truename = truename;
            int edit = UserBL.EditUser(user,password);
            if (edit > 0) return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "修改用户信息成功");
            else return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "修改用户信息失败");
          
        }

        //对外方法名：usersix 参数名：username,companyid
        public WCFJsonData UserHandlerDelete(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func["username"].ToString();  //多个用户名用“，”逗号隔开
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            int valid = StringHelper.TryGetInt(func["valid"].ToString());

            int status = UserBL.Delete(username, companyid,valid);
            if (status > 0)
            {
                if (valid>0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "账号激活成功");
                }
                else
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "删除用户成功");
                }
                
            } 
            else return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "操作失败");
          
        }

        //对外方法名：userseven 参数名：companyid,companycode，username，search
        public WCFJsonData UserList(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string companycode = func["companycode"].ToString();
            string[] userids = func["username"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var search = func["search"].ToString();

            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search)??new SearchBase();

            

            List<UserCheck> list = UserBL.GetUserList(SearchBase, companyid, companycode, userids);
            return JSONHelper.GetWcfJson(list,(int)EnumHelper.Status.Success, "成功");
           

        }

        //对外方法名：usereight 参数名：companyid,companycode，username，search
        public WCFJsonData UpdatePwd(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func["username"].ToString();//用户名
            string oldpassword = func["oldpassword"].ToString();//原密码
            string password = func["password"].ToString();//新密码

            WCFJsonData result=null;
            string pwd = UserBL.GetUserPassword(username);

            if (string.IsNullOrEmpty(pwd))
            {
               return result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "原密码错误");
            }
            pwd = (EncryptHelper.PasswordToText(pwd) + ConstCommon.WcfPassWordMd5Key);

            pwd = EncryptHelper.GetMd5(pwd);

            if (!string.IsNullOrEmpty(pwd) && oldpassword == pwd)
            {
                password = EncryptHelper.TextToPassword(password); 
                FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
                user.username = username;
                user.userpwd = password;
                int add = UserBL.Update(user);
                result = add > 0 ? JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "修改密码成功") : JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "修改密码失败");
            }
            else {
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "原密码错误");
            }
            return result;
        }
    }
}
