using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.UserOrder
{
    public class UserOrderAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserOrder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserOrder_default",
                "UserOrder/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}