using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FXT.DataCenter.WCF.Services.CustomModules;
using Ninject;
using Ninject.Web.Common;

namespace FXT.DataCenter.WCF.Hosting
{
    /// <summary>
    /// 这里要注意，实现NinjectHttpApplication
    /// </summary>
    public class Global : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            //创建一个IOC容器，并且将服务管理模块与内部接口映射模块添加进去 

            //这个IOC容器创建后由Global管理

            return new StandardKernel(new ServiceModule(), new InternalModule());
            
        }
    }
}
