
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.BLL;

namespace Kingsun.ExamPaper.WeChat.StudyReport
{
    public partial class SpeechEval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindInfo();
            }
        }
        CatalogBLL catalogBll = new CatalogBLL();
        public void bindInfo()
        {
            try
            {
                int ParentID = Request["ParentID"] == null ? 0 : Convert.ToInt32(Request["ParentID"]);
                string ClassID = Request["ClassID"] == null ? "" : Request["ClassID"];
                if (ParentID == 0 || ClassID == "")
                {
                    this.DivMain.InnerHtml = "<h5 style='text-align: center;'>参数错误</h5>";
                    return;
                }
                DataTable dt = catalogBll.GetCatalogStudyByParentID(ParentID).Tables[0];
                string html = "<h2>口语评测</h2>";
                if (dt != null && dt.Rows.Count > 0)
                {
                    html += "  <ul>";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToInt32(row["Num"]) > 0)
                        {
                            html += "<li><a href='RecordList.aspx?ClassID=" + ClassID + "&CatalogID=" + row["CatalogID"] + "'>" + row["CatalogName"] + "</a></li>"; 
                        }
                        else
                        {
                            html += "<li class='noclick'><a href='#'>" + row["CatalogName"] + "</a></li>";
                        }
                    }
                    html += "  </ul>";
                }
                this.DivSpeechEval.InnerHtml = html;
            }
            catch (Exception ex)
            {
                this.DivMain.InnerHtml = "<h5 style='text-align: center;'>操作异常</h5>";
            }
        }
    }
}