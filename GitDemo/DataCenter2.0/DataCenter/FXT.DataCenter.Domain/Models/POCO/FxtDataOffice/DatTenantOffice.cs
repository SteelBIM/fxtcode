using System;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
    //Dat_Tenant_Office
    public class DatTenantOffice
    {

        /// <summary>
        /// 房号租客表
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("租客ID")]
        public long HouseTenantId { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("城市ID")]
        public int CityId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("行政区ID")]
        public int AreaId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("商务中心ID")]
        public int SubAreaId { get; set; }

        [DisplayName("*办公商务中心")]
        public string SubAreaName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("楼盘ID")]
        public long ProjectId { get; set; }

        [DisplayName("*办公楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("楼盘别名")]
        public string ProjectOtherName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("楼栋ID")]
        public long BuildingId { get; set; }

        [DisplayName("*楼栋名称")]
        public string BuildingName { get; set; }

        [DisplayName("楼栋别名")]
        public string BuildingOtherName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("房号ID")]
        public long HouseId { get; set; }

        [DisplayName("楼层")]
        public string FloorNum { get; set; }

        [DisplayName("房号")]
        public string HouseName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租客ID")]
        public long TenantID { get; set; }

        [DisplayName("*租客名称")]
        public string TenantName { get; set; }
                
        [DisplayName("租赁面积_平方米")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("租金单价_元/平方米*月")]
        public decimal? Rent { get; set; }
               
        [ExcelExportIgnore]
        [DisplayName("行业大类code")]
        public int TypeCode { get; set; }

        [DisplayName("*行业大类")]
        public string TypeCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("行业小类code")]
        public int? SubTypeCode { get; set; }

        [DisplayName("行业小类")]
        public string SubTypeCodeName { get; set; }
        
        [DisplayName("进驻时间")]
        public DateTime? JoinDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("是否空置")]
        public int IsVacant { get; set; }

        [DisplayName("是否空置")]
        public string IsVacant1 { get; set; }

        [ExcelExportIgnore]
        [DisplayName("是否典型租客")]
        public int? IsTypical { get; set; }

        [DisplayName("是否典型租客")]
        public string IsTypical1 { get; set; }
        
        [DisplayName("*调查时间")]
        public DateTime SurveyDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("调查时间from")]
        public DateTime SurveyDateFrom { get; set; }

        [ExcelExportIgnore]
        [DisplayName("调查时间to")]
        public DateTime SurveyDateTo { get; set; }
                
        [DisplayName("调查人")]
        public string SurveyUser { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("FxtCompanyId")]
        public int FxtCompanyId { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("创建人ID")]
        public string Creator { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("是否有效")]
        public int Valid { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("保存时间")]
        public DateTime? SaveDateTime { get; set; }
        
        [ExcelExportIgnore]
        [DisplayName("修改人")]
        public string SaveUser { get; set; }
        
        [DisplayName("备注")]
        public string Remarks { get; set; }

    }
}