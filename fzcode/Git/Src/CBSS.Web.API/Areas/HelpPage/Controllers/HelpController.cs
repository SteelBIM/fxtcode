using System;
using System.Web.Http;
using System.Web.Mvc;
using CBSS.Web.API.Areas.HelpPage.Models;

namespace CBSS.Web.API.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpController.HelpController()”的 XML 注释
        public HelpController()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpController.HelpController()”的 XML 注释
            : this(GlobalConfiguration.Configuration)
        {
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpController.HelpController(HttpConfiguration)”的 XML 注释
        public HelpController(HttpConfiguration config)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpController.HelpController(HttpConfiguration)”的 XML 注释
        {
            Configuration = config;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpController.Configuration”的 XML 注释
        public HttpConfiguration Configuration { get; private set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpController.Configuration”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpController.Index()”的 XML 注释
        public ActionResult Index()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpController.Index()”的 XML 注释
        {
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpController.Api(string)”的 XML 注释
        public ActionResult Api(string apiId)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpController.Api(string)”的 XML 注释
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View("Error");
        }
    }
}