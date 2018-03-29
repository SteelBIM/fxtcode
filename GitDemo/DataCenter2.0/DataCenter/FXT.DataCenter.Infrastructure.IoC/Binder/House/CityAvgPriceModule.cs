using FXT.DataCenter.Application.Interfaces.House;
using FXT.DataCenter.Application.Services.House;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class CityAvgPriceModule : Module
    {
        public override void Load()
        {
            Bind<ICityAvgPrice>().To<CityAvgPrice>();
            Bind<IDropDownList>().To<DropDownList>();
            Bind<ILog>().To<Log>();
            Bind<ICityAvgPriceServices>().To<CityAvgPriceServices>();
        }
    }
}
