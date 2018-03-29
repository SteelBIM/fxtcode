using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    //Dat_P_B_Price_Office
    public class DatPbPriceOffice
    {
        
        /// <summary>
        /// 办公楼盘楼栋动态价格调查表
        /// </summary>
        [ExcelExportIgnore]
        public long Id { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        [ExcelExportIgnore]
        public int AreaId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("办公楼盘")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public long ProjectId { get; set; }

        [DisplayName("*办公楼盘")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        [DisplayName("办公楼栋")]
        public string BuildingName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("调查方式")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int SurveyTypeCode { get; set; }

        [DisplayName("*调查方式")]
        public string SurveyTypeName { get; set; }
        
        [DisplayName("*平均租金_元每平方米*日")]
        [Required]
        public decimal AvgRent { get; set; }

        [DisplayName("最低日租金_元每平方米")]
        public decimal? Rent1 { get; set; }

        [DisplayName("最高日租金_元每平方米")]
        public decimal? Rent2 { get; set; }

        [DisplayName("平均售价_元每平方米")]
        public decimal? AvgSalePrice { get; set; }

        [DisplayName("最低售价_元每平方米")]
        public decimal? SalePrice1 { get; set; }

        [DisplayName("最高售价_元每平方米")]
        public decimal? SalePrice2 { get; set; }

        [DisplayName("平均租售比_月租金/销售单价")]
        public decimal? RentSaleRate { get; set; }

        [DisplayName("已租面积_平方米")]
        public decimal? TenantArea { get; set; }

        [DisplayName("空置面积_平方米")]
        public decimal? VacantArea { get; set; }

        [DisplayName("空置率_百分比")]
        public decimal? VacantRate { get; set; }

        [DisplayName("管理费_元每平方米*月")]
        public decimal? ManagerPrice { get; set; }

        [DisplayName("*调查时间")]
        [Required]
        public DateTime? SurveyDate { get; set; }

        [DisplayName("调查人")]
        public string SurveyUser { get; set; }
        
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建人")]
        public string Creator { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [ExcelExportIgnore]
        public int Valid { get; set; }

        [ExcelExportIgnore]
        [DisplayName("最后保存时间")]
        public DateTime? SaveDateTime { get; set; }

        [ExcelExportIgnore]
        [DisplayName("最后修改人")]
        public string SaveUser { get; set; }
    }
}