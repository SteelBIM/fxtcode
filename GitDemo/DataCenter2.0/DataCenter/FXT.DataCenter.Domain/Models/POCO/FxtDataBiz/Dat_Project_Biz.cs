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
    /// 商业街道、路段信息表
    /// </summary>
    public class Dat_Project_Biz
    {
        [ExcelExportIgnore]
        public long ProjectId { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        [ExcelExportIgnore]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int AreaId { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [ExcelExportIgnore]
        //[Display(Name = "商圈")]
        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int SubAreaId { get; set; }

        [DisplayName("商圈")]
        public string SubAreaName { get; set; }

        [DisplayName("*商业街")]
        [Required(ErrorMessage = "请填写{0}")]
        public string ProjectName { get; set; }

        [DisplayName("别名")]
        public string OtherName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("商圈关联度ID")]
        public int? CorrelationType { get; set; }

        [DisplayName("商圈关联度")]
        public string CorrelationTypeName { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [ExcelExportIgnore]
        [DisplayName("交通便捷度ID")]
        public int? TrafficType { get; set; }

        [DisplayName("交通便捷度")]
        public string TrafficTypeName { get; set; }

        [DisplayName("交通便捷度描述")]
        public string TrafficDetails { get; set; }

        [ExcelExportIgnore]
        [DisplayName("停车便捷度ID")]
        public int? ParkingLevel { get; set; }

        [DisplayName("停车便捷度")]
        public string ParkingLevelName { get; set; }

        [DisplayName("概况")]
        public string Details { get; set; }

        [DisplayName("开业时间")]
        public string OpenDate { get; set; }

        [DisplayName("土地起始日期")]
        public DateTime? StartDate { get; set; }

        [DisplayName("土地终止日期")]
        public DateTime? StartEndDate { get; set; }

        [DisplayName("区位情况")]
        public string AreaDetails { get; set; }

        [DisplayName("四至东")]
        public string East { get; set; }

        [DisplayName("四至南")]
        public string south { get; set; }

        [DisplayName("四至西")]
        public string west { get; set; }

        [DisplayName("四至北")]
        public string north { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建人")]
        public string Creator { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [ExcelExportIgnore]
        [DisplayName("最后保存时间")]
        public DateTime? SaveDateTime { get; set; }

        [ExcelExportIgnore]
        [DisplayName("最后修改人")]
        public string SaveUser { get; set; }

        [DisplayName("拼音简写")]
        public string PinYin { get; set; }

        [DisplayName("楼盘名称全拼")]
        public string PinYinAll { get; set; }

        [ExcelExportIgnore]
        public int Valid { get; set; }

        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [DisplayName("经度")]
        public decimal? X { get; set; }

        [DisplayName("纬度")]
        public decimal? Y { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("是否标杆")]
        public int IsTypical { get; set; }

        [DisplayName("项目均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? AveragePrice { get; set; }

        [DisplayName("价格系数")]
        [ExcelExportIgnore]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }
    }
}
