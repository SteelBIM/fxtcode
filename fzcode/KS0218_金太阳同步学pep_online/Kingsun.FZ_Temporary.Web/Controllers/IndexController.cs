using Kingsun.FZ_Temporary.BLL;
using Kingsun.FZ_Temporary.Common;
using Kingsun.FZ_Temporary.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


namespace Kingsun.FZ_Temporary.Web.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CollectUserInfoBLL bll = new CollectUserInfoBLL();
        public ActionResult Index()
        { 
            return View();
        }
        public JsonResult AddCollectUserInfo(string Name, string Phone, string FromUrl)
        {
            try
            {
                CollectUserInfoModel model = new CollectUserInfoModel()
                {
                    Name = Name,
                    TelePhone = Phone,
                    FromUrl = FromUrl
                };
                bool flag_Check = bll.CheckCollectUserInfo(Phone);
                if (flag_Check)//手机号存在
                {
                    return Json(new { Success = false, Data = "", Msg = "该手机号已经领取过了！请勿重复领取！" });
                }
                else
                {
                    bool flag = bll.AddCollectUserInfo(model);
                    if (flag)
                    {
                        return Json(new { Success = true, Data = "", Msg = "领取成功！" });
                    }
                    else
                    {
                        return Json(new { Success = false, Data = "", Msg = "领取失败了！" });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "领取失败了！" });
            }

        }
         

        public JsonResult ValidateCode(string telephone)
        {
            try
            {
                int EndMessageCodeTime = 5;
                if (string.IsNullOrEmpty(telephone))
                {
                    var returnObj = new { Success = false, Msg = "电话号码错误!" };
                    return Json(returnObj);
                }
                DataSet ds = bll.GetPhoneCode(telephone);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var returnObj = new { Success = false, Msg = "请使用五分钟内获取的验证码登陆!" };
                    return Json(returnObj);
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
                        if (bll.AddPhoneCode(phonecode))
                        {
                            //string checkCode = Utils.Number(6);
                            //验证码缓存起来 为5分钟有效
                            //if (context.Cache[telephone] != null)
                            //{
                            //    context.Cache.Remove(telephone);
                            //}

                            //context.Cache.Insert(telephone, telephone + "," + phonecode.Code, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration); //这里给数据加缓存，设置缓存时间
                             //SMSService.SMSService smssmessage = new SMSService.SMSService();
                            //string messageContent = ProjectConstant.MessageModel.Replace("[code]", phonecode.Code).Trim();//"您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                            //string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, messageContent);
                            //string[] resultArr = results.Split(',');
                            //if (resultArr[0] == "0" || resultArr[0] == "200")
                            //{
                            //    var returnObj =
                            //        new
                            //        {
                            //            CheckCode = "",
                            //            TelePhone = telephone,
                            //            Success = true,
                            //            Msg = ""
                            //        };
                            //    return Json(returnObj);
                            //}
                            //else
                            //{
                            //    var returnObj = new { Success = false, Msg = "验证码发送失败!" };
                            //    return Json(returnObj);
                            //}
                          
                        }
                        else
                        {
                            var returnObj = new { Success = false, Msg = "验证码发送失败!" };
                            return Json(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "发送验证码异常！" });
            }
            return null;
        }
    }
}
