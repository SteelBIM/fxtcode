namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Ninject;
    using FxtDataAcquisition.Framework.Ioc;

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel(new BaseBinder());
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
    }
}