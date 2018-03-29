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
    public class DatPBPriceIndustry
    {
        /// <summary>
        /// 工业楼盘楼栋动态价格调查表
        /// </summary>
        [ExcelExportIgnore]
        public long Id { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }
        [ExcelExportIgnore]
        [Range(1, int.MaxValue, ErrorMessage = "请选择工业楼盘")]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }
        [DisplayName("*工业楼盘")]
        public string ProjectName { get; set; }
        [DisplayName("工业楼栋")]
        public string BuildingName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("调查方式1148")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择调查方式")]
        public int SurveyTypeCode { get; set; }
        [DisplayName("*调查方式")]
        public string SurveyTypeCodeName { get; set; }

        [DisplayName("*平均租金_元每平方米*日")]
        public decimal AvgRent { get; set; }

        [DisplayName("平均售价_元每平方米")]
        public decimal? AvgSalePrice { get; set; }

        [DisplayName("最低日租金_元每平方米")]
        public decimal? Rent1 { get; set; }

        [DisplayName("最高日租金_元每平方米")]
        public decimal? Rent2 { get; set; }

        [DisplayName("最低售价_元每平方米")]
        public decimal? SalePrice1 { get; set; }

        [DisplayName("最高售价_元每平方米")]
        public decimal? SalePrice2 { get; set; }

        [DisplayName("已租面积_平方米")]
        public decimal? TenantArea { get; set; }

        [DisplayName("空置面积_平方米")]
        public decimal? VacantArea { get; set; }

        [DisplayName("空置率_百分比")]
        public decimal? VacantRate { get; set; }

        [DisplayName("平均租售比_月租金/销售单价")]
        public decimal? RentSaleRate { get; set; }

        [DisplayName("管理费_元每平方米*月")]
        public decimal? ManagerPrice { get; set; }

        [DisplayName("*调查时间")]
        [Required]
        public DateTime? SurveyDate { get; set; }

        [DisplayName("调查人")]
        public string SurveyUser { get; set; }

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
