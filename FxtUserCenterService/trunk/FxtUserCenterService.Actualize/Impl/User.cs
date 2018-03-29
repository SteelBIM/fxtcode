using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;
using System.Text.RegularExpressions;

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
                                    result = JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "成功");
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
                    result = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "用户名无效");
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
            WCFJsonData result = new WCFJsonData();
            string username = func["username"].ToString();
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string password = func["password"].ToString();
            string emailstr = func["emailstr"].ToString();
            string mobile = func["mobile"].ToString();
            string wxopenid = func["wxopenid"].ToString();
            int isinner;
            try
            {
                isinner = string.IsNullOrEmpty(func["isinner"].ToString()) ? 1 : StringHelper.TryGetInt(func["isinner"].ToString());
            }
            catch (Exception ex)
            {
                isinner = 1;
            }
            //验证是否为简单密码
            var appinfo = JObject.Parse(info)["appinfo"];
            result = SecurityVerify.VerifyPassWordIsSimple(appinfo["systypecode"].ToString(), EncryptHelper.PasswordToText(password), username);
            if (result.returntype<0)
            {
                return result;
            }


            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
            user.companyid = companyid;
            user.username = username;
            user.userpwd = password;
            user.emailstr = emailstr;
            user.mobile = mobile;
            user.wxopenid = wxopenid;
            user.isinner = isinner;
            int add = UserBL.Add(user);
            if (add > 0) result= JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "新增用户成功");
            else result= JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增用户失败");
            return result;
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
                        return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "成功");
                    }
                    else { return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "机构无效"); }
                }
                else { return JSONHelper.GetWcfJson(chkuser, (int)EnumHelper.Status.Success, "用户名无效"); }
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
            int isinner;
            try
            {
                isinner = string.IsNullOrEmpty(func["isinner"].ToString()) ? 1 : StringHelper.TryGetInt(func["isinner"].ToString());
            }
            catch (Exception ex)
            {
                isinner = 1;
            }
            WCFJsonData result = new WCFJsonData();

            //验证用户中心用户是否存在
            FxtUserCenterService.Entity.UserInfo user = UserBL.GetUserInfoByUserName(username);//根据用户名获取用户信息
            if (user != null)
            {
                //if (user.valid == 1)//用户已存在
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户已存在");
            }
            else //用户不存在，直接新增
            {
                //验证是否为简单密码
                if (!string.IsNullOrEmpty(password))
                {
                    var appinfo = JObject.Parse(info)["appinfo"];
                    result = SecurityVerify.VerifyPassWordIsSimple(appinfo["systypecode"].ToString(), EncryptHelper.PasswordToText(password), username);
                    if (result.returntype < 0)
                    {
                        return result;
                    }
                }


                user = new FxtUserCenterService.Entity.UserInfo();
                user.companyid = companyid;
                user.username = username;
                user.userpwd = password;
                user.emailstr = emailstr;
                user.mobile = mobile;
                user.wxopenid = wxopenid;
                user.truename = truename;
                user.isinner = isinner;
                int add = UserBL.Add(user);
                result= add > 0 ? JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "新增用户成功") : JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增用户失败");
                return result;
            }
        }

        //对外方法名：userfive 参数名：username,companyid,emailstr,mobile,wxopenid,truename
        public WCFJsonData UserHandlerEdit(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];
            WCFJsonData result = new WCFJsonData();
            string username = Convert.ToString(func["username"]);
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string emailstr = Convert.ToString(func["emailstr"]);
            string mobile = Convert.ToString(func["mobile"]);
            string wxopenid = Convert.ToString(func["wxopenid"]);
            string truename = Convert.ToString(func["truename"]);
            //传递密码后，需要修改用户密码
            string password = Convert.ToString(func["password"]);

            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
            //验证是否为简单密码
            
            if (!string.IsNullOrEmpty(password))
            {
                var appinfo = JObject.Parse(info)["appinfo"];
                result = SecurityVerify.VerifyPassWordIsSimple(appinfo["systypecode"].ToString(), EncryptHelper.PasswordToText(password), username);
                if (result.returntype < 0)
                {
                    return result;
                }
            }
            


            UserCheck chkuser = UserBL.GetFindUser(username);
            if (chkuser == null) return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户名不存在");
            else
                user.companyid = companyid;
            user.username = username;
            //user.userpwd = userpwd;
            user.emailstr = emailstr;
            user.mobile = mobile;
            user.wxopenid = wxopenid;
            user.truename = truename;
            int edit = UserBL.EditUser(user, password);
            if (edit > 0) result= JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "修改用户信息成功");
            else result= JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "修改用户信息失败");
            return result;
        }

        //对外方法名：usersix 参数名：username,companyid
        public WCFJsonData UserHandlerDelete(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            string username = func.Value<string>("username");  //多个用户名用“，”逗号隔开
            int companyid = StringHelper.TryGetInt(func.Value<string>("companyid"));
            int valid = StringHelper.TryGetInt(func.Value<string>("valid"));

            int status = UserBL.Delete(username, companyid, valid);
            if (status > 0)
            {
                if (valid > 0)
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
            string keyword = Convert.ToString(func["keyword"]);
            int? valid = null;
            if (!string.IsNullOrEmpty(Convert.ToString(func["valid"])))
            {
                valid = Convert.ToInt32(func["valid"]);
            }

            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var list = UserBL.GetUserList(SearchBase, companyid, companycode, valid, keyword, userids).Select(o => new
            {
                username = o.username,
                companyid = o.companyid,
                uservalid = o.uservalid,
                emailstr = o.emailstr,
                mobile = o.mobile,
                truename = o.truename,
                wxopenid = o.wxopenid,
                companyname = o.companyname,
                recordcount = o.recordcount

            });

            return JSONHelper.GetWcfJson(list, (int)EnumHelper.Status.Success, "成功");


        }

        //对外方法名：usereight 参数名：companyid,companycode，username，search
        public WCFJsonData UpdatePwd(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];
            
            string username = func["username"].ToString();//用户名
            string oldpassword = func["oldpassword"].ToString();//原密码
            string password = func["password"].ToString();//新密码

            WCFJsonData result = null;
            string pwd = UserBL.GetUserPassword(username);

            if (string.IsNullOrEmpty(pwd))
            {
                return result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "原密码错误");
            }
            pwd = (EncryptHelper.PasswordToText(pwd) + ConstCommon.WcfPassWordMd5Key);

            pwd = EncryptHelper.GetMd5(pwd);

            if (!string.IsNullOrEmpty(pwd) && oldpassword == pwd)
            {
                //验证是否为简单密码
                var appinfo = JObject.Parse(info)["appinfo"];
               
                result = SecurityVerify.VerifyPassWordIsSimple(appinfo["systypecode"].ToString(), password, username);
                if (result.returntype<0) 
                {
                    return result;
                }
                password = EncryptHelper.TextToPassword(password);
                FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
                user.username = username;
                user.userpwd = password;
                int add = UserBL.Update(user);
                result = add > 0 ? JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "修改密码成功") : JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "修改密码失败");
            }
            else
            {
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "原密码错误");
            }
            return result;
        }

        /// <summary>
        /// 修改用户密码(供接口调用,不验证原密码的正确性)
        /// zhoub 20160527
        /// </summary>
        /// <returns></returns>
        public WCFJsonData UpdateUserPwd(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];
            var appinfo = JObject.Parse(info)["appinfo"];
            string username = func["username"].ToString();//用户名
            string password = func["password"].ToString();//新密码
            WCFJsonData result = null;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "参数错误");
            }

            string oldpassword = UserBL.GetUserPassword(username);

            if (string.IsNullOrEmpty(oldpassword))
            {
                return result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户不存在");
            }

            //验证是否为简单密码
            result = SecurityVerify.VerifyPassWordIsSimple(appinfo["systypecode"].ToString(), password, username);
            if (result.returntype < 0)
            {
                return result;
            }
            password = EncryptHelper.TextToPassword(password);
            FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
            user.username = username;
            user.userpwd = password;
            int add = UserBL.Update(user);
            result = add > 0 ? JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "修改密码成功") : JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "修改密码失败");
            return result;
        }

        /// <summary>
        /// 修改用户密码 hody 2014-07-25
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int UpdatePassWord(string username, string userpwd, string oldpwd)
        {
            return UserBL.UpdatePassWord(username, userpwd, oldpwd);
        }

        /// <summary>
        /// 根据用户名获取用户真实姓名（多个用户名用逗号隔开）
        /// zhoub 20160908
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetUserInfoByUserNames(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            string username = func["username"].ToString();
            if (string.IsNullOrEmpty(username))
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户名不能为空");
            }
            string[] array = username.TrimEnd(',').Split(',');
            string newusername="";
            foreach(string name in array)
            {
                newusername += "'" + name + "'" + ",";
            }
            var companyList = UserBL.GetUserInfoByUserNames(newusername.TrimEnd(',')).Select(s => new
            {
                username=s.username,
                truename = s.truename
            }).ToList();

            if (companyList.Count>0)
            {
                return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "未查询到任何数据");
            }
        }

        /// <summary>
        /// 根据公司ID、用户名查询有效的客户帐号信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetUserListByUserName(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string username = Convert.ToString(func["username"]);
            var search = func["search"].ToString();
            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var list = UserBL.GetUserListByUserName(SearchBase, companyid, username).Select(s => new
            {
                username = s.username,
                companyid=s.companyid,
                createdate=s.createdate,
                valid=s.valid,
                userpwd=s.userpwd,
                emailstr=s.emailstr,
                mobile=s.mobile,
                wxopenid=s.wxopenid,
                updatedate=s.updateDate,
                truename=s.truename,
                isinner=s.isinner,
                recordcount=s.recordcount
            });
            return JSONHelper.GetWcfJson(list, (int)EnumHelper.Status.Success, "成功");
        }

        /// <summary>
        /// 根据公司ID、用户名或真实姓名查询客户帐号信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetUserListByUserNameOrTrueName(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            var uInfo = JObject.Parse(info)["uinfo"];

            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            string username = Convert.ToString(func["username"]);
            var search = func["search"].ToString();
            var SearchBase = JSONHelper.JSONToObject<SearchBase>(search) ?? new SearchBase();

            var list = UserBL.GetUserListByUserNameOrTrueName(SearchBase, companyid, username).Select(s => new
            {
                username = s.username,
                companyid=s.companyid,
                createdate=s.createdate,
                valid=s.valid,
                userpwd=s.userpwd,
                emailstr=s.emailstr,
                mobile=s.mobile,
                wxopenid=s.wxopenid,
                updatedate=s.updateDate,
                truename=s.truename,
                isinner=s.isinner,
                recordcount=s.recordcount
            });
            return JSONHelper.GetWcfJson(list, (int)EnumHelper.Status.Success, "成功");
        }
    }
}
