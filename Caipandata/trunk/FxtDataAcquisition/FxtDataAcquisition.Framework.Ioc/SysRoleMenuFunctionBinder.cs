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
    public class SysRoleMenuFunctionBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<ISysRoleMenuFunctionService>().To<SysRoleMenuFunctionService>();
        }
    }
}
