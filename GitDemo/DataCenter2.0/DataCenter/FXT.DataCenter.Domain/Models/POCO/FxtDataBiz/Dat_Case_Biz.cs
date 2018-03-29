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
    /// 商业案例表
    /// </summary>
    public class Dat_Case_Biz
    {
        /// <summary>
        /// 商业案例表
        /// </summary>
        [ExcelExportIgnore]
        public long Id { get; set; }        
        [ExcelExportIgnore]
        public int CityId { get; set; }
        [ExcelExportIgnore]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "请选择行政区")]
        public int AreaId { get; set; }
        [ExcelExportIgnore]
        public int SubAreaId { get; set; }
        [ExcelExportIgnore]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "请选择商业街")]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [ExcelExportIgnore]
        public long HouseId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("商圈")]
        public string SubAreaName { get; set; }

        [DisplayName("*商业街")]
        public string ProjectName { get; set; }

        [DisplayName("商业楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("商业房号")]
        public string HouseName { get; set; }

        [DisplayName("项目地址")]
        public string Address { get; set; }

        [DisplayName("*建筑面积")]
        [Required]
        public decimal BuildingArea { get; set; }

        [DisplayName("*单价")]
        [Required]
        public decimal UnitPrice { get; set; }

        [DisplayName("*总价")]
        public decimal? TotalPrice { get; set; }
        
        [ExcelExportIgnore]
        [Range(1,int.MaxValue,ErrorMessage="请选择案例类型")]
        [DisplayName("案例类型3001")]
        public int CaseTypeCode { get; set; }
        [DisplayName("*案例类型名称")]
        public string CaseTypeName { get; set; }
       
        [ExcelExportIgnore]
        [DisplayName("租金方式1155(出租案例时填写)")]
        public int? RentTypeCode { get; set; }
        [DisplayName("租金方式")]
        public string RentTypeName { get; set; }

        [Required]
        [DisplayName("*案例时间")]
        public DateTime CaseDate { get; set; }

       [DisplayName("租金增长率_百分比/年")]
        public decimal? RentRate { get; set; }

        [DisplayName("商铺类型1182")]
        [ExcelExportIgnore]
        public int HouseType { get; set; }
        [DisplayName("商铺类型")]
        public string HouseTypeName { get; set; }

        [DisplayName("经营业态ID")]
        [ExcelExportIgnore]
        public List<string> BizCodeId { get; set; }
        [DisplayName("经营业态ID多选")]
        [ExcelExportIgnore]
        public string BizCode { get; set; }
        [DisplayName("经营业态")]
        public string BizCodeName { get; set; }

        [DisplayName("所在楼层")]
        public string FloorNo { get; set; }

        [DisplayName("总楼层")]
        public int? TotalFloor { get; set; }

        [DisplayName("装修情况1125")]
        [ExcelExportIgnore]
        public int Fitment { get; set; }
        [DisplayName("装修情况")]
        public string FitmentName { get; set; }

        [DisplayName("物业费_元/平方米*月")]
        public decimal? ManagerPrice { get; set; }

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
        public decimal BuildingAreaFrom { get; set; }
        [ExcelExportIgnore]
        public decimal BuildingAreaTo { get; set; }
        [ExcelExportIgnore]
        public decimal UnitPriceFrom { get; set; }
        [ExcelExportIgnore]
        public decimal UnitPriceTo { get; set; }
    }
}
