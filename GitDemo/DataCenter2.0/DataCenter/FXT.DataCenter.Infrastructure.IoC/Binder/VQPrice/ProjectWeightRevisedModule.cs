using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Application.Services.VQPrice;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class ProjectWeightRevisedModule : Module
    {
        public override void Load()
        {
            Bind<IProjectWeightRevised>().To<ProjectWeightRevised>();
            Bind<ILog>().To<Log>();
            Bind<IProjectCase>().To<ProjectCase>();
            Bind<ICompany>().To<Company>();
            Bind<IImportTask>().To<ImportTask>();
            Bind<IProjectWeightRevisedServices>().To<ProjectWeightRevisedServices>();
        }
    }
}
