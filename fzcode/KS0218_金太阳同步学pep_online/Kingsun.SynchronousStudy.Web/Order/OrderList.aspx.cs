using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.BLL;

namespace Kingsun.AppLibrary.Web.OrderManager
{
    public partial class OrderList : System.Web.UI.Page
    {
        OrderImplement implete = new OrderImplement();
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
                    request.Function = "QueryList";
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
                case "excel":
                    OrderManagement om = new OrderManagement();
                    if (Request.QueryString["queryStr"] == null)
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    MemoryStream s = om.Excel(where).ToExcel() as MemoryStream;
                    if (s != null)
                    {
                        byte[] excel = s.ToArray();
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename=订单列表.xlsx"));
                        Response.AddHeader("Content-Length", excel.Length.ToString());
                        Response.BinaryWrite(excel);
                        s.Close();
                        Response.Flush();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}