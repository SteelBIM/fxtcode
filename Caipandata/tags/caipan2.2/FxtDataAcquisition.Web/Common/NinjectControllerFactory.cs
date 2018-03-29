using FxtDataAcquisition.Framework.Ioc;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace FxtDataAcquisition.Web.Common
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel(new BaseBinder());
            //ninjectKernel.Get<UnitOfWork>();
        }

        protected override IController GetControllerInstance(RequestContext
    requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
    }
}