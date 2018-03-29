using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 商业楼栋动态价格调查表
    /// </summary>
    public class Dat_P_B_Price_Biz
    {
        [ExcelExportIgnore]
        public long Id { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        [DisplayName("*商业街")]
        [ExcelExportIgnore]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public long ProjectId { get; set; }

        [DisplayName("*商业街")]
        public string ProjectName { get; set; }

        [DisplayName("商业楼栋")]
        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        [DisplayName("商业楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("*平均租金_元/平方米*日")]
        [Required]
        public decimal AvgRent { get; set; }

        [ExcelExportIgnore]
        [DisplayName("*调查方式")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int SurveyTypeCode { get; set; }

        [DisplayName("*调查方式")]
        public string SurveyTypeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租金方式")]
        public int? RentTypeCode { get; set; }

        [DisplayName("租金方式")]
        public string RentTypeName { get; set; }

        [DisplayName("平均售价_元/平方米")]
        public decimal? AvgSalePrice { get; set; }

        [DisplayName("最低日租金_元/平方米")]
        public decimal? Rent1 { get; set; }

        [DisplayName("最高日租金_元/平方米")]
        public decimal? Rent2 { get; set; }

        [DisplayName("最低售价_元/平方米")]
        public decimal? SalePrice1 { get; set; }

        [DisplayName("最高售价_元/平方米")]
        public decimal? SalePrice2 { get; set; }

        [DisplayName("已租面积_平方米")]
        public decimal? TenantArea { get; set; }

        [DisplayName("空置面积_平方米")]
        public decimal? VacantArea { get; set; }

        [DisplayName("空置率")]
        public decimal? VacantRate { get; set; }

        [DisplayName("平均租售比_月租金/销售单价")]
        public decimal? RentSaleRate { get; set; }

        [DisplayName("管理费_元/平方米*月")]
        public decimal? ManagerPrice { get; set; }

        [DisplayName("调查时间")]
        public DateTime? SurveyDate { get; set; }

        [DisplayName("调查人")]
        public string SurveyUser { get; set; }

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
    }
}
