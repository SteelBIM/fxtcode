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
    public class AllotFlowBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IAllotFlowService>().To<AllotFlowService>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
