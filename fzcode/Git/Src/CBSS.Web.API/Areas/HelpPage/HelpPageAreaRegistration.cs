using System.Web.Http;
using System.Web.Mvc;

namespace CBSS.Web.API.Areas.HelpPage
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration���� XML ע��
    public class HelpPageAreaRegistration : AreaRegistration
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration.AreaName���� XML ע��
        public override string AreaName
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration.AreaName���� XML ע��
        {
            get
            {
                return "HelpPage";
            }
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration.RegisterArea(AreaRegistrationContext)���� XML ע��
        public override void RegisterArea(AreaRegistrationContext context)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��HelpPageAreaRegistration.RegisterArea(AreaRegistrationContext)���� XML ע��
        {
            context.MapRoute(
                "HelpPage_Default",
                "Help/{action}/{apiId}",
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}