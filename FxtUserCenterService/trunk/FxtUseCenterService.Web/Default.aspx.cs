using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.Entity;
using FxtUseCenterService.Web.Library;

namespace FxtUserCenterService.Hosting
{
    
    public partial class _default : System.Web.UI.Page
    {
        public static int returntype = 0;
        protected void Page_Load(object sender, EventArgs e)
        { 
            string token = Request["token"],
                   np =Request["np"];
            WCFJsonData jsonData = new WCFJsonData();
            jsonData = Public.GetAPIPwdCheckResult(np, token, HttpContext.Current);
            returntype = jsonData.returntype;
        }
    }
}