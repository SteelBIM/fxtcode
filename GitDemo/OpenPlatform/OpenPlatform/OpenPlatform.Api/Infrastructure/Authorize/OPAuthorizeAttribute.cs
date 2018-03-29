using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace OpenPlatform.Api.Infrastructure.Authorize
{
    public class OpAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal == null) return false;

            var userName = principal.Claims.Single(c => c.Type == "username").Value;
            var ip = principal.Claims.Single(c => c.Type == "ip").Value;
            var clientIp = actionContext.Request.GetOwinContext().Request.RemoteIpAddress;

            return true;
        }
    }
}