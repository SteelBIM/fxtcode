using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.CourseManagement
{
    public partial class RevisionHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            VersionChangeBLL versionChangeBLL = new VersionChangeBLL();
            switch (action)
            {
                case "querymodulelist":
                    int totalcount = 0;
                    IList<TB_VersionChange> modulelist = new List<TB_VersionChange>();
                    if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                    {
                        var obj1 = new { rows = modulelist, total = totalcount };
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
                    }
                    modulelist = versionChangeBLL.GetModuleByID(where);
                    if (modulelist == null)
                    {
                        modulelist = new List<TB_VersionChange>();
                    }
                    else
                    {
                        totalcount = modulelist.Count;
                        IList<TB_VersionChange> removelist = new List<TB_VersionChange>();
                        for (int i = 0; i < modulelist.Count; i++)
                        {
                            if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                            {
                                removelist.Add(modulelist[i]);
                            }
                        }
                        if (removelist != null && removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                modulelist.Remove(removelist[i]);
                            }
                        }
                    }
                    var obj = new { rows = modulelist, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "changestate":
                    TB_VersionChange versionChange = new TB_VersionChange();
                    string ModuleID = Request.Form["ModuleID"];
                    string state = Request.Form["State"];
                    where = " ID = '" + ModuleID + "'";
                    IList<TB_VersionChange> versionChangeList = versionChangeBLL.GetModuleByID(where);
                    versionChange = versionChangeList[0];
                    versionChange.State = state == "false" ? true : false;
                    bool result = versionChangeBLL.UpdateModuleInfo(versionChange);
                    Response.Write(JsonHelper.EncodeJson(new { result = result }));
                    Response.End();
                    break;
                default:
                    break;
            }
        }
    }
}