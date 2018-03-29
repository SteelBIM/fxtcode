using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Application.Services;
using OpenPlatform.Domain.Repositories;
using OpenPlatform.Framework.Data.Repositories;

namespace OpenPlatform.Framework.IoC
{
   public class CustomerServiceBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<ICustomerRepository>().To<CustomerRepository>();
            Bind<ICustomerService>().To<CustomerService>();
        }
    }
}
