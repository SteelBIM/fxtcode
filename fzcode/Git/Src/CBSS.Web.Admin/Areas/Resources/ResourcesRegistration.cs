using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.ResourcesManager
{
    public class ResourcesManagerRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ResourcesManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ResourcesManager_default",
                "ResourcesManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}