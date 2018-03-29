using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatBuildingIndustry
    {
        /// <summary>
        /// 工业楼栋表
        /// </summary>
        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }
        [DisplayName("*工业楼盘")]
        public string ProjectName { get; set; }
        [DisplayName("*工业楼栋")]
        public string BuildingName { get; set; }
        [DisplayName("别名")]
        public string OtherName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("工业类型ID6013")]
        public int? IndustryType { get; set; }
        [DisplayName("工业类型")]
        public string IndustryTypeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("用途ID")]
        public int? PurposeCode { get; set; }
        [DisplayName("用途")]
        public string PurposeCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("建筑结构ID")]
        public int? StructureCode { get; set; }
        [DisplayName("建筑结构")]
        public string StructureCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("建筑类型ID")]
        public int? BuildingTypeCode { get; set; }
        [DisplayName("建筑类型")]
        public string BuildingTypeCodeName { get; set; }

        [DisplayName("总楼层")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? TotalFloor { get; set; }

        [DisplayName("总高度")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? TotalHigh { get; set; }

        [DisplayName("总建筑面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("竣工日期")]
        public DateTime? EndDate { get; set; }

        [DisplayName("销售日期")]
        public DateTime? SaleDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租售方式ID")]
        public int? RentSaleType { get; set; }
        [DisplayName("租售方式")]
        public string RentSaleTypeName { get; set; }

        [DisplayName("工业面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? IndustryArea { get; set; }

        [DisplayName("工业总层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? IndustryFloor { get; set; }

        [DisplayName("裙楼层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? PodiumBuildingNum { get; set; }

        [DisplayName("地下室层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? BasementNum { get; set; }

        [DisplayName("功能分布")]
        public string Functional { get; set; }

        [DisplayName("大堂面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? LobbyArea { get; set; }

        [DisplayName("大堂层高")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? LobbyHigh { get; set; }

        [ExcelExportIgnore]
        [DisplayName("大堂装修ID1140")]
        public int? LobbyFitment { get; set; }
        [DisplayName("大堂装修")]
        public string LobbyFitmentName { get; set; }

        [DisplayName("客梯数量")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
        public int? LiftNum { get; set; }

        [ExcelExportIgnore]
        [DisplayName("客梯装修ID")]
        public int? LiftFitment { get; set; }
        [DisplayName("客梯装修")]
        public string LiftFitmentName { get; set; }

        [DisplayName("电梯品牌")]
        public string LiftBrand { get; set; }

        [DisplayName("卫浴品牌")]
        public string ToiletBrand { get; set; }

        [ExcelExportIgnore]
        [DisplayName("公共区域装修ID")]
        public int? PublicFitment { get; set; }
        [DisplayName("公共区域装修")]
        public string PublicFitmentName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("外墙装修ID")]
        public int? WallFitment { get; set; }
        [DisplayName("外墙装修")]
        public string WallFitmentName { get; set; }

        [DisplayName("标准层层高")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? FloorHigh { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("楼栋均价_元每平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? AveragePrice { get; set; }

        [DisplayName("价格系数说明")]
        public string PriceDetail { get; set; }

        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [DisplayName("经度")]
        public decimal? X { get; set; }

        [DisplayName("纬度")]
        public decimal? Y { get; set; }

        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        [ExcelExportIgnore]
        public int Valid { get; set; }  
    }
}
