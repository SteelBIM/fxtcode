using System;

namespace FXT.DataCenter.Domain.Models.QueryObjects.House
{
    public class ExceptionCaseParams
    {
        public int AreaId { get; set; }
        public DateTime? CaseDateFrom { get; set; }
        public DateTime? CaseDateTo { get; set; }
        public int PurposeCode { get; set; }
        public decimal? BuildingAreaFrom { get; set; }
        public decimal? BuildingAreaTo { get; set; }
        public int BuildingTypeCode { get; set; }
        public int HouseTypeCode { get; set; }
        public int FrontCode { get; set; }
        public string BuildingDate { get; set; }
        public string Zhuangxiu { get; set; }
        public decimal Uprate { get; set; }
        public decimal Downrate { get; set; }
        public int ProjectId { get; set; }

        public int cityId { get; set; }
        public int fxtCompanyId { get; set; }
        public decimal AvgPrice { get; set; }
        public string SaveUserName { get; set; }
    }
}
