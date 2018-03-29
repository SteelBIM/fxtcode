using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtUserCenterService.Logic;
using CAS.Entity.DBEntity;
using System.Net;
using System.Text;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// sendsms 的摘要说明
    /// 手机短信api
    /// </summary>
    public class sendsms : HttpHandlerBase
    {
        public string GetHttpPost(string url, string sEncoding)
        {
            try
            {
                WebClient WC = new WebClient();
                WC.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                int p = url.IndexOf("?");
                string sData = url.Substring(p + 1);
                url = url.Substring(0, p);
                byte[] postData = Encoding.GetEncoding(sEncoding).GetBytes(sData);
                byte[] responseData = WC.UploadData(url, "POST", postData);
                string ct = Encoding.GetEncoding(sEncoding).GetString(responseData);
                return ct;

            }
            catch (Exception Ex)
            {
                LogHelper.Error(Ex);
                return Ex.Message;
            }
        }

        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] {"fxtcompanyid","username", "mobile", "content" })) return;
            string result = "";
            try
            {
                int fxtcompanyid = StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
                if (fxtcompanyid <= 0) return;
                CompanyInfo company = CompanyBL.Get(fxtcompanyid);
                if (company == null) return;

                //调用接口发消息
                string urlx = "http://mb345.com:999/ws/Send2.aspx?CorpID={0}&Pwd={1}&Mobile={2}&Content={3}&Cell={4}&SendTime={5}";
                //账号，可以自己设置，传递自己的账号。
                string corpID = "LKSDK0002571";
                //kevin 20130729
                if (!string.IsNullOrEmpty(company.smsloginname))
                {
                    corpID = company.smsloginname;
                }
                else if(!string.IsNullOrEmpty(GetRequest("corpid"))){
                    corpID = GetRequest("corpid");
                }
                //密码
                string pwd = "034712";
                //kevin 20130729
                if (!string.IsNullOrEmpty(company.smsloginpassword))
                {
                    pwd = company.smsloginpassword;
                }
                else if(!string.IsNullOrEmpty(GetRequest("pwd"))){
                    pwd = GetRequest("pwd");
                }
                //接收手机
                string mobile = GetRequest("mobile");
                //消息内容
                string sbContent = HttpUtility.UrlDecode(GetRequest("content"));
                //kevin 20130729
                if (!string.IsNullOrEmpty(company.smssendname)) {
                    sbContent += "【"+company.smssendname+"】";
                }
                string cell = GetRequest("cell");
                string sendtime = GetRequest("sendtime");
                urlx = string.Format(urlx,corpID,pwd,mobile,sbContent,cell,sendtime);
                int sendType = StringHelper.TryGetInt(GetHttpPost(urlx,"gb2312"));
                //写数据库
                DatPhoneMessage msg = new DatPhoneMessage();
                msg.cityid = StringHelper.TryGetInt(GetRequest("cityid"));
                msg.fxtcompanyid = StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
                msg.companyid = StringHelper.TryGetInt(GetRequest("companyid"));
                msg.extracode=cell;
                msg.issucceed=sendType;
                msg.isused=true;
                msg.messcode=StringHelper.TryGetInt(GetRequest("messcode"));
                msg.objectid=StringHelper.TryGetInt(GetRequest("objectid"));
                if(!string.IsNullOrEmpty(GetRequest("overdate")))
                    msg.overdate=StringHelper.TryGetDateTime(GetRequest("overdate"));                
                msg.pascode="";
                msg.phoneno=mobile;
                //msg.sendtime=DateTime.Now;
                msg.surveyid=StringHelper.TryGetInt(GetRequest("sid"));
                msg.text=sbContent;
                msg.typecode=StringHelper.TryGetInt(GetRequest("typecode"));
                msg.userid=GetRequest("username");
                DatPhoneMessageBL.Add(msg);
                result = GetJson(sendType, "");
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