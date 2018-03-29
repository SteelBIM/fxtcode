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
    /// 商业房号表
    /// </summary>
    public class Dat_House_Biz
    {
        /// <summary>
        /// 商业房号表
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("商业房号ID")]
        public long HouseId { get; set; }

        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [DisplayName("商业街")]
        [ExcelExportIgnore]
        public int ProjectId { get; set; }
        [DisplayName("商业街")]
        public string ProjectName { get; set; }
        [DisplayName("商业街别名")]
        public string ProjectOtherName { get; set; }

        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [DisplayName("楼栋名称")]
        public string BuildName { get; set; }

        [ExcelExportIgnore]
        public long FloorId { get; set; }
        [DisplayName("物理层")]
        public int FloorNo { get; set; }
        [DisplayName("实际层")]
        public string FloorNum { get; set; }

        [DisplayName("城市")]
        [ExcelExportIgnore]
        public int CityId { get; set; }
        [DisplayName("城市")]
        [ExcelExportIgnore]
        public string CityName { get; set; }

        [DisplayName("室号")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string UnitNo { get; set; }

        [DisplayName("房号名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string HouseName { get; set; }

        [DisplayName("证载用途")]
        [ExcelExportIgnore]
        public int? PurposeCode { get; set; }
        [DisplayName("证载用途")]
        public string PurposeCodeName { get; set; }

        [DisplayName("实际用途")]
        [ExcelExportIgnore]
        public int? SJPurposeCode { get; set; }
        [DisplayName("实际用途")]
        public string SJPurposeCodeName { get; set; }

        [DisplayName("建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("套内面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? InnerBuildingArea { get; set; }

        [DisplayName("朝向")]
        [ExcelExportIgnore]
        public int? FrontCode { get; set; }
        [DisplayName("朝向")]
        public string FrontCodeName { get; set; }

        [DisplayName("平面形状")]
        [ExcelExportIgnore]
        public int? Shape { get; set; }
        [DisplayName("平面形状")]
        public string ShapeName { get; set; }

        [DisplayName("开间")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Width { get; set; }

        [DisplayName("进深")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Length { get; set; }

        [DisplayName("有无夹层")]
        [ExcelExportIgnore]
        public int? IsMezzanine { get; set; }
        [DisplayName("有无夹层")]
        public string IsMezzanineName { get; set; }

        [DisplayName("商业阻隔")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int BizCutOff { get; set; }
        [DisplayName("商业阻隔")]
        public string BizCutOffName { get; set; }

        [DisplayName("商铺类型ID")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int BizHouseType { get; set; }
        [DisplayName("商铺类型")]
        public string BizHouseTypeName { get; set; }

        [DisplayName("商铺位置类型ID")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int BizHouseLocation { get; set; }
        [DisplayName("商铺位置类型")]
        public string BizHouseLocationName { get; set; }

        [DisplayName("临街、位置描述")]
        public string Location { get; set; }

        [ExcelExportIgnore]
        [DisplayName("人流量ID")]
        public int? FlowType { get; set; }
        [DisplayName("人流量")]
        public string FlowTypeName { get; set; }

        [DisplayName("开门数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        public int? DoorNum { get; set; }

        [DisplayName("出租率")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,4}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? RentRate { get; set; }

        [DisplayName("单价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? UnitPrice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,4}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }

        [ExcelExportIgnore]
        public int? IsEValue { get; set; }
        [DisplayName("是否可估")]
        public string IsEValueName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("评估机构")]
        public int FxtCompanyId { get; set; }

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

        [ExcelExportIgnore]
        [DisplayName("是否有效")]
        public int Valid { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }
    }
}
