using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Framework.Data.EntityFramework;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Framework.Ioc
{
    public class BuildingBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IBuildingService>().To<BuildingService>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IHouseService>().To<HouseService>();
        }
    }
}
