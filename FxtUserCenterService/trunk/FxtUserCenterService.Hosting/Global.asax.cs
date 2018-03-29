using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FxtUserCenterService.Actualize;
using System.Web.Routing;
using System.ServiceModel.Activation;
using CAS.Common;

namespace FxtUserCenterService.Hosting
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //招商 0337AF0F-EF62-4D54-B264-716CA5D770C3  杭州 3E60ECE3-5A44-4039-BB3A-DE7FF5482F9C
            //string key = "100320160606";
            //string[] certifyArray = { "1003104", "10030606", "3E60ECE3-5A44-4039-BB3A-DE7FF5482F9C", "20160823115442", "housedropdownlistmcas" };
            //string code=Validator.GetApiFunctionArgsVerifyCode(certifyArray,key);
            RouteTable.Routes.Add(new ServiceRoute("uc", new WebServiceHostFactory(), typeof(UserCenter)));
            RouteTable.Routes.Add(new ServiceRoute("gt", new WebServiceHostFactory(), typeof(GeTui)));  
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}