using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Framework.Data.EntityFramework;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Framework.Ioc
{
    public class BaseEntityBinder : NinjectModule
    {
        public override void Load()
        {
            //Bind<IDbContextFactory>().To<DbContextFactory>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            
        }
    }
}
