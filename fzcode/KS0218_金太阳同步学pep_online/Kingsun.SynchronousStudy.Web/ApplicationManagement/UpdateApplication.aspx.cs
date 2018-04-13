using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common;
using Kingsun.PSO;
using System.Net;

namespace Kingsun.SynchronousStudy.Web.ApplicationManagement
{
    public partial class UpdateApplication : System.Web.UI.Page
    {
        public string menuList = "";
        public ClientUserinfo UserInfo = new ClientUserinfo();
        ApplicationVersionBLL applicationBLL = new ApplicationVersionBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo = CheckLogin.Check(HttpContext.Current, ref menuList);
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "updateapp":
                    TB_ApplicationVersion appInfo = new TB_ApplicationVersion();
                    int versionID = Convert.ToInt32(Request.Form["VersionID"]);
                    string versionType = Request.Form["VersionType"];
                    string versionNumber = "V" + Request.Form["VersionNum"];
                    string description = Request.Form["Content"];
                    string userName = Request.Form["Name"];
                    string fileAddress = Request.Form["Address"];
                    string fileMD5 = Request.Form["VerifyValue"];
                    string mandatoryUpdate = Request.Form["MandatoryUpdate"];
                    if (Utils.filterSql(description + "," + fileMD5 + "," + fileAddress))
                    {
                        string msg = "有SQL攻击嫌疑，请停止操作。";
                        Response.Write(JsonHelper.EncodeJson(new { obj = false, result = msg }));
                        Response.End();
                    }
                    if (versionType == "IOS")
                    {
                        appInfo.VersionType = 1;
                    }
                    else
                    {
                        appInfo.VersionType = 2;
                    }
                    description = HTMLEncode(description);
                    appInfo.VersionID = versionID;
                    appInfo.VersionNumber = versionNumber;
                    appInfo.VersionDescription = description.Replace(System.Environment.NewLine, "");
                    appInfo.UserName = userName;
                    appInfo.FileAddress = fileAddress;
                    appInfo.FileMD5 = fileMD5;
                    appInfo.State = false;
                    if (mandatoryUpdate == "true")
                    {
                        appInfo.MandatoryUpdate = true;
                    }
                    else
                    {
                        appInfo.MandatoryUpdate = false;
                    }
                    bool result = applicationBLL.InsertApp(appInfo);
                    Response.Write(JsonHelper.EncodeJson(new { obj = result, result = "更新失败" }));
                    Response.End();
                    break;
                case "getmaxversionnum":
                    string where = "";
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    IList<TB_ApplicationVersion> IOSList = applicationBLL.QueryAppList(where + " and VersionType = 1");
                    IList<TB_ApplicationVersion> AndroidList = applicationBLL.QueryAppList(where + " and VersionType != 1");
                    var resultObj = new { IOSList = IOSList, AndroidList = AndroidList };
                    Response.Write(JsonHelper.EncodeJson(resultObj));
                    Response.End();
                    break;
                default:
                    break;
            }
        }

        //替换敏感字符，防止js脚本注入
        public static string HTMLEncode(string txt)
        {
            string Ntxt = txt;
            Ntxt = Ntxt.Replace(" ", "&nbsp;");
            Ntxt = Ntxt.Replace("<", "&lt;");
            Ntxt = Ntxt.Replace(">", "&gt;");
            Ntxt = Ntxt.Replace("\"", "&quot;");
            Ntxt = Ntxt.Replace("'", "&#39;");
            Ntxt = Ntxt.Replace("\n", "<br>");
            return Ntxt;
        }
    }
}