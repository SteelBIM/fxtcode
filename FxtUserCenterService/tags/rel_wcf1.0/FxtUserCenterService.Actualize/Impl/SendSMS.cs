using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;
using System.Net;
using System.Web;
using CAS.Entity.DBEntity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        //对外方法名：ss_func_1 参数名：fxtcompanyid,corpid,password,mobile,content,cell,sendtime,cityid,companyid,messcode,objectid,overdate,sid,typecode,username,
        public WCFJsonData SendSMS(string sinfo, string info)
        {
            var uInfo = JObject.Parse(info)["uinfo"];
            var func = JObject.Parse(info)["funinfo"];
            var appinfo = JObject.Parse(info)["appinfo"];

            int fxtcompanyid = StringHelper.TryGetInt(uInfo["fxtcompanyid"].ToString());
            var corpid = func["corpid"].ToString();
            var pswd = uInfo["password"].ToString();
            var mobile = func["mobile"].ToString();
            var content = func["content"].ToString();
            var cell = func["cell"].ToString();
            var sendtime = func["sendtime"].ToString();
            var cityid = func["cityid"].ToString();
            var companyid = func["companyid"].ToString();
            var messcode = func["messcode"].ToString();
            var objectid = func["objectid"].ToString();
            var overdate = func["overdate"].ToString();
            var sid = func["sid"].ToString();
            var typecode = func["typecode"].ToString();
            var username = uInfo["username"].ToString();

            //int producttypecode = StringHelper.TryGetInt(appinfo["systypecode"].ToString());//产品类型
            //int actionid = StringHelper.TryGetInt(func["actionid"].ToString());//功能编号

           
            if (fxtcompanyid <= 0)  return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "失败");
            CompanyInfo company = CompanyBL.Get(fxtcompanyid);
            if (company == null) return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "失败"); 

            //调用接口发消息
            string urlx = "http://mb345.com:999/ws/Send2.aspx?CorpID={0}&Pwd={1}&Mobile={2}&Content={3}&Cell={4}&SendTime={5}";
            //账号，可以自己设置，传递自己的账号。
            string corpID = "LKSDK0002571";
            //kevin 20130729
            if (!string.IsNullOrEmpty(company.smsloginname))
            {
                corpID = company.smsloginname;
            }
            else if (!string.IsNullOrEmpty(corpid))
            {
                corpID = corpid;
            }
            //密码
            string pwd = "034712";
            //kevin 20130729
            if (!string.IsNullOrEmpty(company.smsloginpassword))
            {
                pwd = company.smsloginpassword;
            }
            else if (!string.IsNullOrEmpty(pswd))
            {
                pwd = pswd;
            }
            //接收手机
            //string mobile = mobile1;
            //消息内容
            string sbContent = HttpUtility.UrlDecode(content);
            //kevin 20130729
            if (!string.IsNullOrEmpty(company.smssendname))
            {
                sbContent += "【" + company.smssendname + "】";
            }
            //string cell = GetRequest("cell");
            //string sendtime = GetRequest("sendtime");
            urlx = string.Format(urlx, corpID, pwd, mobile, sbContent, cell, sendtime);
            int sendType = StringHelper.TryGetInt(GetHttpPost(urlx, "gb2312"));
            //写数据库
            DatPhoneMessage msg = new DatPhoneMessage();
            msg.cityid = StringHelper.TryGetInt(cityid);
            msg.fxtcompanyid = fxtcompanyid;
            msg.companyid = StringHelper.TryGetInt(companyid);
            msg.extracode = cell;
            msg.issucceed = sendType;
            msg.isused = true;
            msg.messcode = StringHelper.TryGetInt(messcode);
            msg.objectid = StringHelper.TryGetInt(objectid);
            if (!string.IsNullOrEmpty(overdate))
                msg.overdate = StringHelper.TryGetDateTime(overdate);
            msg.pascode = "";
            msg.phoneno = mobile;
            //msg.sendtime=DateTime.Now;
            msg.surveyid = StringHelper.TryGetInt(sid);
            msg.text = sbContent;
            msg.typecode = StringHelper.TryGetInt(typecode);
            msg.userid = username;
            DatPhoneMessageBL.Add(msg);
            return JSONHelper.GetWcfJson(sendType, (int)EnumHelper.Status.Success, "成功");

        }

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
    }
}
