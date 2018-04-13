using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SunnyTask.Web.Account
{
    public partial class SunnyStudentFoot : System.Web.UI.UserControl
    {
        public string FootUserID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Kingsun.PSO.PSOCookie cookie = new PSO.PSOCookie(HttpContext.Current);
            //Kingsun.PSO.ClientUserinfo clientUser = cookie.GetCookieUserInfo();
            //if (clientUser == null)
            //{
            //    FootUserID = "";
            //}
            //else
            //{
            //    FootUserID = clientUser.UserID;
            //}
        }
    }
}