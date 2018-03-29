using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Repositories;
using FxtDataAcquisition.Framework.Data.EntityFramework;
using FxtDataAcquisition.Framework.Data.EntityFramework.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Ioc
{
    public class ProjectAllotFlowSurveyBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IProjectAllotFlowSurveyService>().To<ProjectAllotFlowSurveyService>();
            Bind<IProjectAllotFlowSurveyRepository>().To<ProjectAllotFlowSurveyRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
