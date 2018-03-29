using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Application.Services.VQPrice;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class HouseWeightRevisedModule : Module
    {

        public override void Load()
        {
            Bind<IHouseWeightRevised>().To<HouseWeightRevised>();
            Bind<ILog>().To<Log>();
            Bind<IDAT_House>().To<DAT_HouseDAL>();
            Bind<IHouseWeightRevisedServices>().To<HouseWeightRevisedServices>();
        }
    }
}
