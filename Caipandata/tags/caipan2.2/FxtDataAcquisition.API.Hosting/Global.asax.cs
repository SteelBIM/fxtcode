using AutoMapper;
using FxtDataAcquisition.API.Service.APIActualize;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace FxtDataAcquisition.API.Hosting
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Init(object sender, EventArgs e)
        {

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute("mobileapi", new WebServiceHostFactory(), typeof(FxtMobileAPI)));

            Mapper.CreateMap<Project, ProjectDto>();
            Mapper.CreateMap<Building, BuildingDto>();
            Mapper.CreateMap<House, HouseDto>();
            Mapper.CreateMap<ProjectDto, Project>();
            Mapper.CreateMap<BuildingDto, Building>();
            Mapper.CreateMap<HouseDto, House>();
            Mapper.CreateMap<Feedback, FeedbackDto>();
            Mapper.CreateMap<House, HouseDetails>();
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