using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Application.Interfaces.Share;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Application.Services.Share
{
    public class MarketAnalysisServices : IMarketAnalysisServices
    {
        private readonly IDropDownList _dropDownList;
        private readonly ILog _log;
        private readonly IOfficeSubArea _officeSubArea;
        private readonly IIndustrySubArea _industrySubArea;
        private readonly IMarketAnalysis _market;

        public MarketAnalysisServices(IDropDownList dropDownList, ILog log, IMarketAnalysis market, IOfficeSubArea officeSubArea, IIndustrySubArea industrySubArea)
        {
            _dropDownList = dropDownList;
            _officeSubArea = officeSubArea;
            _industrySubArea = industrySubArea;
            _log = log;
            _market = market;
        }

        public IQueryable<DatAnalysisMarket> GetAnalysisList(DatAnalysisMarket model, bool self = true)
        {
            return _market.GetAnalysisList(model, self);
        }

        public int AddAnalysis(DatAnalysisMarket model)
        {
            return _market.AddAnalysis(model);
        }

        public bool Update(DatAnalysisMarket model)
        {
            return _market.Update(model);
        }

        public DatAnalysisMarket GetAnalysisById(int id, int dataCode, int cityId, int fxtcompanyId)
        {
            return _market.GetAnalysisById(id, dataCode, cityId, fxtcompanyId);
        }

        public IList<CompanyProduct_Module> GetAreaName(int cityId)
        {
            return _dropDownList.GetAreaName(cityId);
        }
        
        public IList<CompanyProduct_Module> GetSubAreaName(int areaId)
        {
            return _dropDownList.GetSubAreaName(areaId);
        }

        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }

        public IQueryable<SYS_SubArea_Office> GetOfficeSubAreaNamesByAreaId(int areaid, int fxtCompanyId, int CityId)
        {
            return _officeSubArea.GetSubAreaNamesByAreaId(areaid, fxtCompanyId, CityId);
        }

        public IQueryable<SYS_SubArea_Industry> GetIndustrySubAreaNamesByAreaId(int areaid, int fxtCompanyId, int CityId)
        {
            return _industrySubArea.GetSubAreaNamesByAreaId(areaid, fxtCompanyId, CityId);
        }
    }
}
