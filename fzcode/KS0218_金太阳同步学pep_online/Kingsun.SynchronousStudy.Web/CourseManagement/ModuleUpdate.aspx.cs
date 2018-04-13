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
using System.Net;


namespace Kingsun.SynchronousStudy.Web.CourseManagement
{
    public partial class ModuleUpdate : System.Web.UI.Page
    {
        ModuleConfigurationBLL moduleConfigurationBLL = new ModuleConfigurationBLL();
        VersionChangeBLL versionChangeBLL = new VersionChangeBLL();
        ModularSortBLL modularSortBLL = new ModularSortBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            string bookId;
            string firstTitleId;
            string secondTitleId;
            string moduleId;
            switch (action)
            {
                case "getmoduledetails":
                    TB_ModuleSort moduleSort = new TB_ModuleSort();
                    bookId = Request.Form["BookID"];
                    firstTitleId = Request.Form["FirstTitleID"];
                    secondTitleId = Request.Form["SecondTitleID"];
                    moduleId = Request.Form["ModuleID"];
                    string where;
                    if (secondTitleId == "")
                    {
                        where = " BookID = '" + bookId + "' and FirstTitleID = '" + firstTitleId + "' and ModuleID='" + moduleId + "'";
                    }
                    else
                    {
                        where = " BookID = '" + bookId + "' and FirstTitleID = '" + firstTitleId + "' and SecondTitleID = '" + secondTitleId + "' and ModuleID='" + moduleId + "'";
                    }

                    IList<TB_ModuleSort> versionList = modularSortBLL.GetModuleList(where);
                    if (versionList != null && versionList.Count > 0)
                    {
                        moduleSort = versionList[0];
                    }
                    var obj = new { data = moduleSort };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "getbookdetails":
                    TB_ModuleConfiguration moduleConfiguration = new TB_ModuleConfiguration();
                    bookId = Request.Form["BookID"];
                    firstTitleId = Request.Form["FirstTitleID"];
                    secondTitleId = Request.Form["SecondTitleID"];
                    //where = " FirstTitileID = '" + FirstTitleID + "' and SecondTitleID = '" + SecondTitleID + "' and BookID='" + bid + "'";
                    if (secondTitleId == "")
                    {
                        where = " FirstTitileID = '" + firstTitleId + "' and BookID='" + bookId + "'";
                    }
                    else
                    {
                        where = " FirstTitileID = '" + firstTitleId + "' and SecondTitleID = '" + secondTitleId + "' and BookID='" + bookId + "'";
                    }

                    IList<TB_ModuleConfiguration> moduleList = moduleConfigurationBLL.GetModuleList(where);
                    if (moduleList != null && moduleList.Count > 0)
                    {
                        moduleConfiguration = moduleList[0];
                    }
                    var obj1 = new { data = moduleConfiguration };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                    Response.End();
                    break;
                case "updatemodule":
                    bookId = Request.Form["BookID"];
                    moduleId = Request.Form["ModuleID"];
                    string moduleName = Request.Form["ModuleName"];
                    string teachingNaterialName = Request.Form["TeachingNaterialName"];
                    string firstTitle = Request.Form["FirstTitle"];
                    firstTitleId = Request.Form["FirstTitleID"];
                    string secondTitle = Request.Form["SecondTitle"];
                    secondTitleId = Request.Form["SecondTitleID"];
                    string moduleAddress = Request.Form["ModuleAddress"];
                    string moduleMd5 = Request.Form["ModuleMD5"];
                    string addModuleAddress = Request.Form["AddModuleAddress"];
                    string addModuleMd5 = Request.Form["AddModuleMD5"];
                    string moduleVersion = "V" + Request.Form["ModuleVersion"];
                    string description = Request.Form["Description"];
                    description = HTMLEncode(description);
                    string mandatoryUpdate = Request.Form["MandatoryUpdate"];
                    if (Utils.filterSql(description + "," + moduleMd5 + "," + addModuleMd5))
                    {
                        string msg = "有SQL攻击嫌疑，请停止操作。";
                        Response.Write(JsonHelper.EncodeJson(new { obj = false, result = msg }));
                        Response.End();
                    }
                    TB_VersionChange versionInfo = new TB_VersionChange();
                    versionInfo.BooKID = ParseInt(bookId);
                    versionInfo.ModuleID = ParseInt(moduleId);
                    versionInfo.ModuleName = moduleName;
                    versionInfo.TeachingNaterialName = teachingNaterialName;
                    versionInfo.FirstTitle = firstTitle;
                    versionInfo.FirstTitleID = ParseInt(firstTitleId);
                    versionInfo.SecondTitle = secondTitle;
                    versionInfo.SecondTitleID = ParseInt(secondTitleId);
                    versionInfo.ModuleAddress = moduleAddress;
                    versionInfo.MD5 = moduleMd5;
                    versionInfo.IncrementalPacketAddress = addModuleAddress;
                    versionInfo.IncrementalPacketMD5 = addModuleMd5;
                    versionInfo.ModuleVersion = moduleVersion;
                    versionInfo.UpdateDescription = description;
                    versionInfo.State = true;
                    if (mandatoryUpdate == "true")
                    {
                        versionInfo.IsUpdate = true;
                    }
                    else
                    {
                        versionInfo.IsUpdate = false;
                    }
                    versionInfo.CreateDate = DateTime.Now;
                    //if (!IsExist(versionInfo.ModuleAddress))
                    //{
                    //    string msg = "模块地址不存在，请检查";
                    //    Response.Write(JsonHelper.EncodeJson(new { obj = false, result = msg }));
                    //    Response.End();
                    //}
                    //if (versionInfo.IncrementalPacketAddress != "")
                    //{
                    //    if (!IsExist(versionInfo.IncrementalPacketAddress))
                    //    {
                    //        string msg = "增量包地址不存在，请检查";
                    //        Response.Write(JsonHelper.EncodeJson(new { obj = false, result = msg }));
                    //        Response.End();
                    //    }
                    //}
                    bool result = versionChangeBLL.InsertModuleInfo(versionInfo);
                    Response.Write(JsonHelper.EncodeJson(new { obj = result, result = "更新失败" }));
                    Response.End();
                    break;
                case "getmaxversionnum":
                    bookId = Request.Form["BookID"];
                    firstTitleId = Request.Form["FirstTitleID"];
                    secondTitleId = Request.Form["SecondTitleID"];
                    moduleId = Request.Form["ModuleID"];
                    where = " BookID = '" + bookId + "' and FirstTitleID = '" + firstTitleId + "' and SecondTitleID = '" + secondTitleId + "' and ModuleID = '" + moduleId + "'";
                    int totalcount = 0;
                    IList<TB_VersionChange> applist = versionChangeBLL.GetModuleByID(where);
                    if (applist == null)
                    {
                        applist = new List<TB_VersionChange>();
                    }
                    else
                    {
                        totalcount = applist.Count;
                    }
                    var obj2 = new { rows = applist, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj2));
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
            catch
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
            if (string.IsNullOrEmpty(txt)) return "";
          
            string Ntxt = txt;
            Ntxt = Ntxt.Replace(" ", "&nbsp;");
            Ntxt = Ntxt.Replace("<", "&lt;");
            Ntxt = Ntxt.Replace(">", "&gt;");
            Ntxt = Ntxt.Replace("\"", "&quot;");
            Ntxt = Ntxt.Replace("'", "&#39;");
            Ntxt = Ntxt.Replace("\n", "<br>");
            return Ntxt;
        }

        /// <summary>
        /// 转换Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }
    }
}