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
    public class DatTenantIndustry
    {
        /// <summary>
        /// 房号租客表
        /// </summary>
        [ExcelExportIgnore]
        public long HouseTenantId { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [ExcelExportIgnore]
        public int SubAreaId { get; set; }
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [ExcelExportIgnore]
        public long HouseId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("工业片区")]
        public string SubAreaName { get; set; }

        [DisplayName("*工业楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("楼盘别名")]
        public string ProjectOtherName { get; set; }

        [DisplayName("*工业楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("楼栋别名")]
        public string BuildingOtherName { get; set; }

        [DisplayName("楼层")]
        public string FloorNum { get; set; }

        [DisplayName("房号")]
        public string HouseName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租客（商家）CompanyId")]
        public long TenantID { get; set; }
        [DisplayName("*租客名称")]
        public string TenantName { get; set; }

        [DisplayName("租赁面积_平方米")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("租金单价_元每平方米*月")]
        public decimal? Rent { get; set; }

        [ExcelExportIgnore]
        [DisplayName("行业大类1158")]
        public int TypeCode { get; set; }
        [DisplayName("*行业大类")]
        public string TypeCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("行业小类1159~1177")]
        public int? SubTypeCode { get; set; }
        [DisplayName("行业小类")]
        public string SubTypeCodeName { get; set; }

        [DisplayName("进驻时间")]
        public DateTime? JoinDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("是否空置")]
        public int IsVacant { get; set; }
        [DisplayName("是否空置")]
        public string IsVacantName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("是否典型租客")]
        public int? IsTypical { get; set; }
        [DisplayName("是否典型租客")]
        public string IsTypicalName { get; set; }

        [DisplayName("*调查时间")]
        public DateTime SurveyDate { get; set; }

        [DisplayName("调查人")]
        public string SurveyUser { get; set; }

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

        [ExcelExportIgnore]
        public DateTime SurveyDateFrom { get; set; }
        [ExcelExportIgnore]
        public DateTime SurveyDateTo { get; set; }  
    }
}
