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
    /// 商业楼栋表
    /// </summary>
    public class Dat_Building_Biz
    {
        /// <summary>
        /// 商业楼栋表
        /// </summary>
        [DisplayName("商业楼栋ID")]
        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        [ExcelExportIgnore]
        public long CityId { get; set; }
        [DisplayName("城市")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int SubAreaId { get; set; }
        [DisplayName("商圈")]
        public string SubAreaName { get; set; }

        [ExcelExportIgnore]
        public int? ProjectId { get; set; }
        [DisplayName("商业街")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("与商务中心的关联度ID")]
        public int? CorrelationType { get; set; }
        [DisplayName("与商务中心的关联度")]
        public string CorrelationTypeName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DisplayName("楼栋名称")]
        public string BuildingName { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("宗地号")]
        public string FieldNo { get; set; }

        [ExcelExportIgnore]
        [DisplayName("建筑结构ID")]
        public int? StructureCode { get; set; }
        [DisplayName("建筑结构")]
        public string StructureCodeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("建筑类型ID")]
        public int? BuildingTypeCode { get; set; }
        [DisplayName("建筑类型")]
        public string BuildingTypeCodeName { get; set; }

        [DisplayName("管理费")]
        public string ManagerPrice { get; set; }

        [DisplayName("管理处电话")]
        public string ManagerTel { get; set; }

        [DisplayName("总建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }

        [DisplayName("竣工日期")]
        public DateTime? EndDate { get; set; }

        [DisplayName("开业日期")]
        public DateTime? OpenDate { get; set; }

        [ExcelExportIgnore]
        [DisplayName("租售方式ID")]
        public int? RentSaleType { get; set; }
        [DisplayName("租售方式")]
        public string RentSaleTypeName { get; set; }

        [DisplayName("商业总面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BizArea { get; set; }

        [DisplayName("商业总层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? BizFloor { get; set; }

        [DisplayName("地上商业层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? UpFloorNum { get; set; }

        [DisplayName("地下商业层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? DownFloorNum { get; set; }

        [DisplayName("功能分布")]
        public string Functional { get; set; }

        [DisplayName("商铺总数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? BizNum { get; set; }

        [DisplayName("客梯数量")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? LiftNum { get; set; }

        [ExcelExportIgnore]
        [DisplayName("客梯装修ID")]
        public int? LiftFitment { get; set; }
        [DisplayName("客梯装修")]
        public string LiftFitmentName { get; set; }

        [DisplayName("电梯品牌")]
        public string LiftBrand { get; set; }

        [DisplayName("扶手电梯数量")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? EscalatorsNum { get; set; }

        [DisplayName("扶手电梯品牌")]
        public string EscalatorsBrand { get; set; }

        [DisplayName("卫浴品牌")]
        public string ToiletBrand { get; set; }

        [ExcelExportIgnore]
        [DisplayName("公共区域装修ID")]
        public int? PublicFitment { get; set; }
        [DisplayName("公共区域装修")]
        public string PublicFitmentName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("外墙装修ID")]
        public int? WallFitment { get; set; }
        [DisplayName("外墙装修")]
        public string WallFitmentName { get; set; }

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

        [ExcelExportIgnore]
        [DisplayName("空调系统类型ID")]
        public int? AirConditionType { get; set; }
        [DisplayName("空调系统类型")]
        public string AirConditionTypeName { get; set; }

        [DisplayName("商业描述")]
        public string Details { get; set; }

        [DisplayName("住宅楼盘库")]
        [ExcelExportIgnore]
        public long? HouseID { get; set; }

        [DisplayName("商业楼盘库")]
        [ExcelExportIgnore]
        public long? OfficeID { get; set; }

        [DisplayName("四至东")]
        public string East { get; set; }

        [DisplayName("四至南")]
        public string south { get; set; }

        [DisplayName("四至西")]
        public string west { get; set; }

        [DisplayName("四至北")]
        public string north { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        [DisplayName("楼栋商业类型ID")]
        public int BuildingBizType { get; set; }
        [DisplayName("楼栋商业类型")]
        public string BuildingBizTypeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("主营商业种类ID")]
        public int? BizType { get; set; }
        [DisplayName("主营商业种类")]
        public string BizTypeName { get; set; }

        [DisplayName("营业时间")]
        public string BusinessHours { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        [DisplayName("临路类型ID")]
        public int ProRoad { get; set; }
        [DisplayName("临路类型")]
        public string ProRoadName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        [DisplayName("商业阻隔ID")]
        public int BizCutOff { get; set; }
        [DisplayName("商业阻隔")]
        public string BizCutOffName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        [DisplayName("人流量ID")]
        public int Flows { get; set; }
        [DisplayName("人流量")]
        public string FlowsName { get; set; }

        [DisplayName("客户消费定位ID")]
        [ExcelExportIgnore]
        public int? CustomerType { get; set; }
        [DisplayName("客户消费定位")]
        public string CustomerTypeName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [Range(0, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        [DisplayName("是否商业标杆")]
        public int IsBenchmarks { get; set; }
        [DisplayName("是否商业标杆")]
        public string IsBenchmark { get; set; }

        [ExcelExportIgnore]
        [DisplayName("创建人")]
        public string Creator { get; set; }

        private DateTime _CreateTime = DateTime.Now;

        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        [ExcelExportIgnore]
        [DisplayName("最后保存时间")]
        public DateTime? SaveDateTime { get; set; }

        [ExcelExportIgnore]
        [DisplayName("最后修改人")]
        public string SaveUser { get; set; }

        [ExcelExportIgnore]
        [DisplayName("拼音简写")]
        public string PinYin { get; set; }

        [ExcelExportIgnore]
        [DisplayName("楼栋名称全拼")]
        public string PinYinAll { get; set; }

        private int _Valid = 1;

        [ExcelExportIgnore]
        [DisplayName("是否有效")]
        public int Valid
        {
            get { return _Valid; }
            set { _Valid = value; }
        }

        [ExcelExportIgnore]
        [DisplayName("评估机构")]
        public int FxtCompanyId { get; set; }

        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        [DisplayName("经度")]
        public decimal? X { get; set; }

        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        [DisplayName("纬度")]
        public decimal? Y { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [ExcelExportIgnore]
        [DisplayName("住宅楼盘")]
        public long? ZZProjectId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("办公楼盘")]
        public long? BGProjectId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("开业结束日期")]
        public DateTime? OpenDateEnd { get; set; }

        [DisplayName("楼栋均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? AveragePrice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }
    }
}
