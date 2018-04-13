using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SunnyTask.Web.Account
{
    public partial class SunnyTeachFoot : System.Web.UI.UserControl
    {
        protected string Teach = string.Empty;
        protected string Task = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Task"]))
                    this.Task = System.Configuration.ConfigurationManager.AppSettings["Task"] + "/";
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Teach"]))
                    this.Teach = System.Configuration.ConfigurationManager.AppSettings["Teach"] + "/";
            }
        }
    }
}