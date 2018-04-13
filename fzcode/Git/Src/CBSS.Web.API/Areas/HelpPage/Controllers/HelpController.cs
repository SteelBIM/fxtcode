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
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.HelpController()���� XML ע��
        public HelpController()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.HelpController()���� XML ע��
            : this(GlobalConfiguration.Configuration)
        {
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.HelpController(HttpConfiguration)���� XML ע��
        public HelpController(HttpConfiguration config)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.HelpController(HttpConfiguration)���� XML ע��
        {
            Configuration = config;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Configuration���� XML ע��
        public HttpConfiguration Configuration { get; private set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Configuration���� XML ע��

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Index()���� XML ע��
        public ActionResult Index()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Index()���� XML ע��
        {
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Api(string)���� XML ע��
        public ActionResult Api(string apiId)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpController.Api(string)���� XML ע��
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