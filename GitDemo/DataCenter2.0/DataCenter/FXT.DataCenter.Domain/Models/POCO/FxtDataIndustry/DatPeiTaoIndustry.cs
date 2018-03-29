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
    public class DatPeiTaoIndustry
    {
        /// <summary>
        /// 工业配套
        /// </summary>
        [ExcelExportIgnore]
        public long PeiTaoID { get; set; }
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public int? CityId { get; set; }
        [ExcelExportIgnore]
        public int? AreaId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("*工业楼盘")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("配套类型1149")]
        public int PeiTaoCode { get; set; }
        [DisplayName("*配套类型")]
        public string PeiTaoCodeName { get; set; }

        [DisplayName("*配套名称")]
        public string PeiTaoName { get; set; }

        [DisplayName("*楼层")]
        public string Floor { get; set; }

        [DisplayName("部位")]
        public string Location { get; set; }

        [DisplayName("面积_平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租户（商家）CompanyID")]
        public long TenantID { get; set; }
        [DisplayName("*商家名称")]
        public string TenantName { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [ExcelExportIgnore]
        public string Creators { get; set; }
        [ExcelExportIgnore]
        public DateTime? CreateDate { get; set; }
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveDate { get; set; }
        [ExcelExportIgnore]
        public int? FxtCompanyId { get; set; }
        [ExcelExportIgnore]
        public int Valid { get; set; }       
    }
}
