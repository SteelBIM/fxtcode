using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseActivate.Web.Admin.Startup))]
namespace CourseActivate.Web.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
