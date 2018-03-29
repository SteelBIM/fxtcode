using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FXT.DataCenter.Domain.Models
{
    //Dat_House_Office
    public class DatHouseOffice
    {

        public long HouseId { get; set; }

        public long BuildingId { get; set; }
        public string BuildingName { get; set; }

        public long ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int CityId { get; set; }

        [DisplayName("*物理层")]
        public int FloorNo { get; set; }

        [DisplayName("实际层")]
        public string FloorNum { get; set; }

        [DisplayName("单元号")]
        public string UnitNo { get; set; }

        [DisplayName("*室号")]
        public string HouseNo { get; set; }

        [Required(ErrorMessage = "房号名称 必填")]
        [DisplayName("房号名称")]
        public string HouseName { get; set; }

        public int? PurposeCode { get; set; }
        public string PurposeName { get; set; }

        public int? SJPurposeCode { get; set; }
        public string SJPurposeName { get; set; }

        [DisplayName("建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("套内面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? InnerBuildingArea { get; set; }

        public int? FrontCode { get; set; }
        public string FrontName { get; set; }

        public int? SightCode { get; set; }
        public string SightName { get; set; }

        [DisplayName("单价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? UnitPrice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }

        [DisplayName("是否可估")]
        public int? IsEValue { get; set; }
        public string IsEvalueName { get; set; }

        public string Remarks { get; set; }

        public int FxtCompanyId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? SaveDateTime { get; set; }
        public string SaveUser { get; set; }
        public int Valid { get; set; }
    }
}