using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FxtUserCenterService.Logic;
using FxtUserCenterService.Entity;
using CAS.Common;
using CAS.Entity;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// user_check 的摘要说明
    /// 检查登录用户有效 kevin 2013-4-2
    /// </summary>
    public class user_check : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            //caoq 2013-11-28 不验证 systypecode （手机云查勘是通用版，需要登录后根据开通产品验证code）
            if (!CheckMustRequest(new string[] { "username" })) return;
            string result = "";
            string username = GetRequest("username");
            // string userpwd = GetRequest("userpwd");
            //string companyid = GetRequest("companyid");
            int systypecode = StringHelper.TryGetInt(GetRequest("systypecode"));

            try
            {
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
                                        result = GetJson(chkuser);
                                    }
                                    else
                                    {
                                        result = GetJson(0, "产品已超过有效期");
                                    }
                                }
                                else
                                {
                                    result = GetJson(0, "产品无效");
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
                                    result = GetJson(0, "产品无效");
                                }
                                else if (prolist.Where(o => o.overdate > DateTime.Now).Count() <= 0)
                                {
                                    result = GetJson(0, "产品已超过有效期");
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
                                    result = GetJson(chkuser);
                                }
                            }
                        }
                        else
                        {
                            result = GetJson(0, "机构无效");
                        }
                    }
                    else
                    {
                        result = GetJson(0, "用户名无效");
                    }
                }
                else
                {
                    result = GetJson(0, "用户名不存在");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = GetJson(ex);
            }
            context.Response.Write(result);
        }

    }
}