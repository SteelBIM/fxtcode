using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Framework.Data.EntityFramework;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Ioc
{
    public class ProjectBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IProjectService>().To<ProjectService>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IHouseService>().To<HouseService>();
        }
    }
}
