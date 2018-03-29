using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Application.Services.VQPrice;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class ProjectAvgPriceModule : Module
    {
        public override void Load()
        {
            Bind<IProjectAvgPrice>().To<ProjectAvgPrice>();
            Bind<ILog>().To<Log>();
            Bind<IImportTask>().To<ImportTask>();
            Bind<IProjectAvgPriceServices>().To<ProjectAvgPriceServices>();
        }
    }
}
