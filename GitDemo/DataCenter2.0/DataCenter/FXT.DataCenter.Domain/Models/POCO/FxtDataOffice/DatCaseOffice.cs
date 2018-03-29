using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    //Dat_Case_Office
    public class DatCaseOffice
    {
        /// <summary>
        /// 办公案例表
        /// </summary>
        [ExcelExportIgnore]
        public long Id { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [ExcelExportIgnore]
        [Range(1, int.MaxValue, ErrorMessage = "请选择办公楼盘！")]
        public long ProjectId { get; set; }

        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        [ExcelExportIgnore]
        public long HouseId { get; set; }

        [DisplayName("*办公楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("办公楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("房号")]
        public string HouseName { get; set; }

        [DisplayName("*建筑面积_平方米")]
        public decimal BuildingArea { get; set; }

        [DisplayName("*单价_元每平方米")]
        public decimal UnitPrice { get; set; }

        [DisplayName("总价_元")]
        public decimal? TotalPrice { get; set; }

        [DisplayName("案例种类：出租、出售案例")]
        [ExcelExportIgnore]
        public int CaseType { get; set; }

        [DisplayName("案例类型")]
        [ExcelExportIgnore]
        [Range(1, int.MaxValue, ErrorMessage = "请选择案例类型！")]
        public int CaseTypeCode { get; set; }

        [DisplayName("*案例类型")]
        public string CaseTypeName { get; set; }

        [DisplayName("*案例时间")]
        public DateTime CaseDate { get; set; }

        [DisplayName("办公楼等级1125")]
        [ExcelExportIgnore]
        public int? OfficeType { get; set; }
        [DisplayName("办公楼等级")]
        public string OfficeTypeName { get; set; }

        [DisplayName("装修情况1125")]
        [ExcelExportIgnore]
        public int? Fitment { get; set; }
        [DisplayName("装修情况")]
        public string FitmentName { get; set; }

        [DisplayName("所在楼层")]
        public string FloorNo { get; set; }

        [DisplayName("总楼层")]
        public int? TotalFloor { get; set; }

        [DisplayName("物业费_元/平方米*月")]
        public decimal? ManagerPrice { get; set; }

        [DisplayName("约定租金增长率_百分比每年")]
        public decimal? RentRate { get; set; }

        [DisplayName("中介公司")]
        public string AgencyCompany { get; set; }

        [DisplayName("中介人员")]
        public string Agent { get; set; }

        [DisplayName("中介电话")]
        public string AgencyTel { get; set; }    

        [DisplayName("来源名称")]
        public string SourceName { get; set; }

        [DisplayName("来源链接")]
        public string SourceLink { get; set; }

        [DisplayName("来源电话")]
        public string SourcePhone { get; set; }

        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }
        [ExcelExportIgnore]
        public int Valid { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }
        [ExcelExportIgnore]
        public string SaveUser { get; set; }

        [ExcelExportIgnore]
        public DateTime CaseDateStart { get; set; }
        [ExcelExportIgnore]
        public DateTime CaseDateEnd { get; set; }
        [ExcelExportIgnore]
        public decimal UnitPriceFrom { get; set; }
        [ExcelExportIgnore]
        public decimal UnitPriceTo { get; set; }
        [ExcelExportIgnore]
        public decimal BuildingAreaFrom { get; set; }
        [ExcelExportIgnore]
        public decimal BuildingAreaTo { get; set; }  
    }
}