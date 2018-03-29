using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CDI.Models
{
    [DataContract]
    [Serializable]
    public class DataCase
    {
        [DataMember]
        public int? ProjectId { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public int? BuildingId { get; set; }
        [DataMember]
        public int? HouseId { get; set; }
        [DataMember]
        public int? CompanyId { get; set; }
        [DataMember]
        public DateTime CaseDate { get; set; }
        [DataMember]
        public int? PurposeCode { get; set; }
        [DataMember]
        public string PurposeName { get; set; }
        [DataMember]
        public int? FloorNumber { get; set; }
        [DataMember]
        public string BuildingName { get; set; }
        [DataMember]
        public string HouseNo { get; set; }
        [DataMember]
        public decimal? BuildingArea { get; set; }
        [DataMember]
        public decimal? UsableArea { get; set; }
        [DataMember]
        public int? FrontCode { get; set; }
        [DataMember]
        public string FrontName { get; set; }
        [DataMember]
        public decimal? UnitPrice { get; set; }
        [DataMember]
        public int? MoneyUnitCode { get; set; }
        [DataMember]
        public string MoneyUnitName { get; set; }
        [DataMember]
        public int? SightCode { get; set; }
        [DataMember]
        public int? CaseTypeCode { get; set; }
        [DataMember]
        public string CaseTypeName { get; set; }
        [DataMember]
        public int? StructureCode { get; set; }
        [DataMember]
        public string StructureName { get; set; }
        [DataMember]
        public int? BuildingTypeCode { get; set; }
        [DataMember]
        public string BuildingTypeName { get; set; }
        [DataMember]
        public int? HouseTypeCode { get; set; }
        [DataMember]
        public string HouseTypeName { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public decimal? TotalPrice { get; set; }
        [DataMember]
        public int? OldID { get; set; }
        [DataMember]
        public int? CityID { get; set; }
        [DataMember]
        public int? Valid { get; set; }
        [DataMember]
        public int? FXTCompanyId { get; set; }
        [DataMember]
        public int? TotalFloor { get; set; }
        [DataMember]
        public int? RemainYear { get; set; }
        [DataMember]
        public decimal? Depreciation { get; set; }
        [DataMember]
        public int? FitmentCode { get; set; }
        [DataMember]
        public string FitmentName { get; set; }
        [DataMember]
        public int? SurveyId { get; set; }
        [DataMember]
        public int? SaveDateTime { get; set; }
        [DataMember]
        public string SaveUser { get; set; }
        [DataMember]
        public string SourceName { get; set; }
        [DataMember]
        public string SourceLink { get; set; }
        [DataMember]
        public string SourcePhone { get; set; }
        [DataMember]
        public int? AreaId { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public string BuildingDate { get; set; }
        [DataMember]
        public string ZhuangXiu { get; set; }
        [DataMember]
        public string SubHouse { get; set; }
        [DataMember]
        public string PeiTao { get; set; }
        [DataMember]
        public DateTime RecordWeek { get; set; }

    }
}
