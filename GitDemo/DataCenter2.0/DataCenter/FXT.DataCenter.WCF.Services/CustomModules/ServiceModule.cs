using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.WCF.Contract;
using Ninject.Modules;

namespace FXT.DataCenter.WCF.Services.CustomModules
{
    /// <summary>
    /// 服务相关的注入模块
    /// </summary>
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            //建立契约与服务之间的映射
            this.Bind<IExcelUpload>().To<ExcelUpload>();
            this.Bind<IResidential>().To<Residential>();
        }
    }
}
