using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryDynamicPrice
    {
        IQueryable<DatPBPriceIndustry> GetDynamicPriceSurveys(DatPBPriceIndustry dynamicPriceSurvey, int pageIndex, int pageSize, out int totalCount, bool self = true);
        DatPBPriceIndustry GetDynamicPriceSurveyById(int id);
        int UpdateDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey);
        int AddDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey);
        int DeleteDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey);
    }
}
