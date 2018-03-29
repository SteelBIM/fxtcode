using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatHouseIndustry
    {
        /// <summary>
        /// 工业房号表
        /// </summary>
        [ExcelExportIgnore]
        public long HouseId { get; set; }
        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }

        [DisplayName("工业楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("工业楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("物理层")]
        public int FloorNo { get; set; }

        [DisplayName("实际层")]
        public string FloorNum { get; set; }

        [DisplayName("单元号")]
        public string UnitNo { get; set; }

        [DisplayName("室号")]
        public string HouseNo { get; set; }

        [DisplayName("房号名称")]
        public string HouseName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("证载用途1002")]
        public int? PurposeCode { get; set; }
        [DisplayName("证载用途")]
        public string PurposeCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("实际用途1002")]
        public int? SJPurposeCode { get; set; }
        [DisplayName("实际用途")]
        public string SJPurposeCodeName { get; set; }

        [DisplayName("建筑面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("套内面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? InnerBuildingArea { get; set; }

        [ExcelExportIgnore]
        [DisplayName("朝向2004")]
        public int? FrontCode { get; set; }
        [DisplayName("朝向")]
        public string FrontCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("景观2006")]
        public int? SightCode { get; set; }
        [DisplayName("景观")]
        public string SightCodeName { get; set; }

        [DisplayName("单价_元每平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? UnitPrice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }

        [ExcelExportIgnore]
        [DisplayName("是否可估")]
        public int? IsEValue { get; set; }
        [DisplayName("是否可估")]
        public string IsEValueName { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        [ExcelExportIgnore]
        public int Valid { get; set; }     
    }
}
