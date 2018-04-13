using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Kingsun.SunnyTask.Common;

namespace Kingsun.SunnyTask.Web.Account
{
    public partial class SunnyStudentHead : System.Web.UI.UserControl
    {
        protected string Teach = string.Empty;
        protected string Task = string.Empty;
        protected string Student = string.Empty;
        protected string display = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Task"]))
                //    this.Task = AppSetting.Task + "/";
                //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Teach"]))
                //    this.Teach = AppSetting.Teach + "/";

                //Kingsun.PSO.PSOCookie cookie = new PSO.PSOCookie(HttpContext.Current);
                //Kingsun.PSO.ClientUserinfo clientUser = cookie.GetCookieUserInfo();
                //if (clientUser == null)
                //{
                //    clientUser = new PSO.ClientUserinfo();
                //}
            }
        }
    }
}