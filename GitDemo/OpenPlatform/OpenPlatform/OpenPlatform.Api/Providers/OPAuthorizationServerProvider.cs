using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Framework.IoC;
using OpenPlatform.Framework.Utils;

namespace OpenPlatform.Api.Providers
{
    public class OpAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var isValidUser = false;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var service = new StandardKernel(new CustomerServiceBinder()).Get<ICustomerService>();
            var customer = service.GetCustomer(context.UserName);
            var password = customer == null ? "" : customer.Password;

            if (EncryptHelper.GetMd5(context.Password, ConfigurationHelper.Encryptkey) == password)
            {
                isValidUser = true;
            }

            if (!isValidUser)
            {
                context.SetError("授权失败", "用户名或密码不正确.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("ip", context.Request.RemoteIpAddress));

            context.Validated(identity);

        }
    }
}