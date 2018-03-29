using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Application.Services.VQPrice;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class BuildingWeightRevisedModule : Module
    {
        public override void Load()
        {
            Bind<IBuildingWeightRevised>().To<BuildingWeightRevised>();
            Bind<ILog>().To<Log>();
            Bind<IDAT_Building>().To<DAT_BuildingDAL>();
            Bind<IProjectWeightRevised>().To<ProjectWeightRevised>();
            Bind<IBuildingWeightRevisedServices>().To<BuildingWeightRevisedServices>();
        }
    }
}
