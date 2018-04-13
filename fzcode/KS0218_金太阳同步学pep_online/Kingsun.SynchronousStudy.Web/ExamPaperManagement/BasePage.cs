using System;
using System.Web;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            this.Load += new EventHandler(BasePage_Load);
        }
        public Kingsun.PSO.ClientUserinfo CurrentUser = null;

        public void WriteResult(object data, string message = "")
        {
            object obj = new { Success = true, Data = data, Message = message };
            Response.Write(JsonHelper.EncodeJson(obj));
            Response.End();
        }
        public void WriteErrorResult(string message = "")
        {
            object obj = new { Success = false, Data = "", Message = message };
            Response.Write(JsonHelper.EncodeJson(obj));
            Response.End();
        }

        public void BasePage_Load(object sender, EventArgs e)
        {
            
        }
    }
}