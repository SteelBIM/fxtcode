using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class AmendPhoneController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /AmendPhone/

        public ActionResult Index()
        {
            string UserID = "";
            if (Request["UserID"] == null || Request["UserID"] == "")
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                return View();
            }
            else
            {
                UserID = Request["UserID"];
            }
            return View();
        }
        PhoneManage phonemange = new PhoneManage();
        public JsonResult SendCode()
        {
            try
            {
                int EndMessageCodeTime = 5;
                string telephone = Request.Form["Telephone"];
                if (string.IsNullOrEmpty(telephone))
                {
                    var returnObj = new { Success = false, Msg = "电话号码错误!" };
                    return Json(returnObj);
                }
                string sql = string.Format("  SELECT TOP 1 * FROM dbo.Tb_PhoneCode WHERE TelePhone='{0}'  AND DATEADD(MINUTE,5,EndDate)>'{1}' AND State=1  ORDER BY EndDate DESC", telephone, DateTime.Now);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Json(new { Success = true, Data = "", Msg = "请使用五分钟内获取的验证码登陆!" });
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
                            SMSService.SMSService smssmessage = new SMSService.SMSService();
                            string messageContent = ProjectConstant.MessageModel.Replace("[code]", phonecode.Code).Trim();//"您的短信验证码为：" + phonecode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";
                            string results = smssmessage.SendMessage(System.Configuration.ConfigurationManager.AppSettings["MessageToken"], telephone, messageContent);
                            string[] resultArr = results.Split(',');
                            if (resultArr[0] == "0" || resultArr[0] == "200")
                            {
                                return Json(new
                                    {
                                        CheckCode = "",
                                        TelePhone = telephone,
                                        Success = true,
                                        Msg = ""
                                    });
                            }
                            else
                            {
                                return Json(new { Success = false, Data = "", Msg = "验证码发送失败!" });
                            }
                        }
                        else
                        {
                            return Json(new { Success = false, Data = "", Msg = "验证码发送失败!" });
                        }
                    }
                }
                return Json(new { Success = false, Data = "", Msg = "验证码发送失败!" });
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常!" });
            }
        }
        /// <summary>
        /// 效验短信验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckCode()
        {
            try
            {
                string telephone = Request.Form["Telephone"];
                string code = Request.Form["Code"];
                if (string.IsNullOrEmpty(code))
                { 
                    return Json(new { Success = false, Msg = "验证码不能为空！" });
                }
                if (string.IsNullOrEmpty(telephone) || telephone == "undefined")
                { 
                    return Json(new { Success = false, Msg = "手机不能为空！" });
                }
                if (phonemange.CheckPhoneCode(telephone, code))
                {
                    return Json(new { Success = true, Data = "", Msg = "" });
                }
                else
                { 
                    return Json(new { Success = false, Msg = "验证码错误！" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常!" });
            }
        }
    }
}
