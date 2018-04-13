using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.BLL;

namespace Kingsun.AppLibrary.Web.FeeManager
{
    public partial class FeeSetting : System.Web.UI.Page
    {
        FeeSettingImplement implete = new FeeSettingImplement();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
                return;
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "querylist":
                    KingRequest request = new KingRequest();
                    request.Function = "QueryFeeComboList";
                    string pageindex = Request.Form["page"];
                    string pagesize = Request.Form["rows"];
                    string where = "";
                    if (Request.QueryString["queryStr"] == null)
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    var obj = new { PageIndex = pageindex, PageSize = pagesize, Where = where };
                    request.Data = JsonHelper.EncodeJson(obj);
                    KingResponse response = implete.ProcessRequest(request);
                    if (response.Success)
                    {
                        Response.Write(JsonHelper.EncodeJson(response.Data));
                        Response.End();
                    }
                    else
                    {
                        Response.Write(response.ErrorMsg);
                        Response.End();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}