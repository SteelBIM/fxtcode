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
    public partial class ApplicationManage : System.Web.UI.Page
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
                case "queryapplist":
                    TB_ApplicationVersion appInfo = new TB_ApplicationVersion();
                    int totalcount = 0;
                    IList<TB_ApplicationVersion> applist = new List<TB_ApplicationVersion>();
                    if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                    {
                        var obj1 = new { rows = applist, total = totalcount };
                        Response.Write(JsonHelper.EncodeJson(obj1));
                        Response.End();
                    }
                    int pageindex = int.Parse(Request.Form["page"].ToString());
                    int pagesize = int.Parse(Request.Form["rows"].ToString());
                    string where;
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        where = "1=1 ORDER BY CreateDate DESC";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                        where += " ORDER BY CreateDate DESC";
                    }

                    applist = applicationBLL.QueryAppList(where);
                    if (applist == null)
                    {
                        applist = new List<TB_ApplicationVersion>();
                    }
                    else
                    {
                        totalcount = applist.Count;
                        IList<TB_ApplicationVersion> removelist = new List<TB_ApplicationVersion>();
                        for (int i = 0; i < applist.Count; i++)
                        {
                            if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                            {
                                removelist.Add(applist[i]);
                            }
                        }
                        if (removelist != null && removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                applist.Remove(removelist[i]);
                            }
                        }
                    }
                    var obj = new { rows = applist, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "changestate":
                    string appID = Request.Form["AppID"];
                    string state = Request.Form["State"];
                    where = " ID = '" + appID + "'";
                    IList<TB_ApplicationVersion> AppList = applicationBLL.GetAppByID(where);
                    appInfo = AppList[0];
                    appInfo.State = state == "false" ? true : false;
                    bool result = applicationBLL.UpdateAppInfo(appInfo);
                    Response.Write(JsonHelper.EncodeJson(new { result = result }));
                    Response.End();
                    break;
                case "getappinfo":
                    appID = Request.Form["AppID"];
                    where = " ID = '" + appID + "'";
                    AppList = applicationBLL.GetAppByID(where);
                    appInfo = AppList[0];
                    Response.Write(JsonHelper.EncodeJson(new { obj = appInfo }));
                    Response.End();
                    break;
                case "changeisenabled":
                    string appId = Request.Form["AppID"];
                    string isEnabled = Request.Form["isEnabled"];
                    where = " ID = '" + appId + "'";
                    IList<TB_ApplicationVersion> AppLists = applicationBLL.GetAppByID(where);
                    appInfo = AppLists[0];
                    appInfo.isEnabled = Convert.ToInt32(isEnabled) == 0 ? 1 : 0;
                    bool rs = applicationBLL.UpdateAppInfo(appInfo);
                    Response.Write(JsonHelper.EncodeJson(new { result = rs }));
                    Response.End();
                    break;
                case "updateapp":
                    appID = Request.Form["AppID"];
                    string versionType = Request.Form["VersionType"];
                    string versionNumber = "V" + Request.Form["VersionNum"];
                    string description = Request.Form["Content"];
                    description = HTMLEncode(description);
                    string fileAddress = Request.Form["Address"];
                    string fileMD5 = Request.Form["VerifyValue"];
                    string mandatoryUpdate = Request.Form["MandatoryUpdate"];
                    where = " ID = '" + appID + "'";
                    if (Utils.filterSql(description + "," + fileMD5 + "," + fileAddress))
                    {
                        string msg = "有SQL攻击嫌疑，请停止操作。";
                        Response.Write(JsonHelper.EncodeJson(new { obj = false, result = msg }));
                        Response.End();
                    }
                    AppList = applicationBLL.GetAppByID(where);
                    appInfo = AppList[0];
                    appInfo.VersionNumber = versionNumber;
                    appInfo.VersionDescription = description;
                    appInfo.FileAddress = fileAddress;
                    appInfo.FileMD5 = fileMD5;
                    if (versionType == "IOS")
                    {
                        appInfo.VersionType = 1;
                    }
                    else
                    {
                        appInfo.VersionType = 2;
                    }
                    if (mandatoryUpdate == "true")
                    {
                        appInfo.MandatoryUpdate = true;
                    }
                    else
                    {
                        appInfo.MandatoryUpdate = false;
                    }
                    result = applicationBLL.UpdateAppInfo(appInfo);
                    Response.Write(JsonHelper.EncodeJson(new { obj = result, result = "更新失败" }));
                    Response.End();
                    break;
                case "getmaxversionnum":
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


        //判断远程文件是否存在 
        public static bool IsExist(string uri)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "HEAD";
                req.Timeout = 100;
                res = (HttpWebResponse)req.GetResponse();
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
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