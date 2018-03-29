using FXT.DataCenter.Application.Interfaces.Share;
using FXT.DataCenter.Application.Services.Share;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.IoC;

namespace FXT.DataCenter.Infrastructure.IoC.Binder
{
    public class MarketAnalysisModule : Module
    {
        public override void Load()
        {
            Bind<IDropDownList>().To<DropDownList>();
            Bind<ILog>().To<Log>();
            Bind<IOfficeSubArea>().To<OfficeSubArea>();
            Bind<IIndustrySubArea>().To<IndustrySubArea>();
            Bind<IMarketAnalysis>().To<MarketAnalysis>();
            Bind<IMarketAnalysisServices>().To<MarketAnalysisServices>();
        }
    }
}
