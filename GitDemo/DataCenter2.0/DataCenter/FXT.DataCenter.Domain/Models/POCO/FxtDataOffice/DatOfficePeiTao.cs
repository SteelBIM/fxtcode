using System;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.ComponentModel.DataAnnotations;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
    //Dat_Office_PeiTao
    public class DatOfficePeiTao
    {

        /// <summary>
        /// 办公配套
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("办公商务配套ID")]
        public long PeiTaoID { get; set; }

        [ExcelExportIgnore]
        [DisplayName("城市ID")]
        public int? CityId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("行政区ID")]
        public int? AreaIdId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("办公楼盘ID")]
        public long ProjectId { get; set; }

        [DisplayName("*办公楼盘")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("配套类型ID")]
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
        [DisplayName("公司id")]
        public int? FxtCompanyId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建者")]
        public string Creators { get; set; }

        [ExcelExportIgnore]
        [DisplayName("修改人")]
        public string SaveUser { get; set; }

        [ExcelExportIgnore]
        [DisplayName("修改时间")]
        public DateTime? SaveDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("有效值")]
        public int? Valid { get; set; }

    }
}