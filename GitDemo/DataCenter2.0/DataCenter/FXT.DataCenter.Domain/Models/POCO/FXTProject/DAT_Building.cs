using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    [Serializable]
    public class DAT_Building
    {
        public DAT_Building()
        {
            this.PriceWeight = 1M;
            this.OldWeight = 0;
        }

        public int cityid { get; set; }
        public int projectid { get; set; }
        public int buildingid { get; set; }

        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("楼栋名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string buildingname { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "请选择楼栋主用途")]
        public int? purposecode { get; set; }
        [DisplayName("楼栋主用途")]
        public string PurposeCodeName { get; set; }

        public int? structurecode { get; set; }
        [DisplayName("建筑结构")]
        public string StructureCodeName { get; set; }

        public int? buildingtypecode { get; set; }
        [DisplayName("建筑类型")]
        public string BuildingTypeCodeName { get; set; }

        [DisplayName("总层数")]
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? totalfloor { get; set; }

        [DisplayName("是否确认总层数")]
        public int? isTotalfloor { get; set; }

        [DisplayName("层高")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? floorhigh { get; set; }

        public string salelicence { get; set; }

        public string elevatorrate { get; set; }

        [DisplayName("单元数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? unitsnumber { get; set; }

        [DisplayName("总户数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? totalnumber { get; set; }

        [DisplayName("建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? totalbuildarea { get; set; }

        public DateTime? builddate { get; set; }

        public DateTime? saledate { get; set; }

        [DisplayName("楼栋均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? averageprice { get; set; }

        [DisplayName("均价层")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? averagefloor { get; set; }

        public DateTime? joindate { get; set; }
        public DateTime? licencedate { get; set; }
        public string othername { get; set; }

        [DisplayName("权重值")]
        [RegularExpression(@"^[0-9]+(.[0-9]{1,4})$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? weight { get; set; }

        public int? isevalue { get; set; }


        [DisplayName("销售均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? saleprice { get; set; }


        public int? locationcode { get; set; }

        public int? sightcode { get; set; }

        public int? frontcode { get; set; }

        private decimal? _structureweight = 0M;
        [DisplayName("楼栋结构修正价格")]
        public decimal? structureweight { get { return _structureweight; } set { _structureweight = value; } }
        private decimal? _buildingtypeweight = 0M;
        [DisplayName("建筑类型修正价格")]
        public decimal? buildingtypeweight { get { return _buildingtypeweight; } set { _buildingtypeweight = value; } }
        private decimal? _yearweight = 0M;
        [DisplayName("年期修正价格")]
        public decimal? yearweight { get { return _yearweight; } set { _yearweight = value; } }
        private decimal? _purposeweight = 0M;
        [DisplayName("用途修正价格")]
        public decimal? purposeweight { get { return _purposeweight; } set { _purposeweight = value; } }
        private decimal? _locationweight = 0M;
        [DisplayName("楼栋位置修正价格")]
        public decimal? locationweight { get { return _locationweight; } set { _locationweight = value; } }
        private decimal? _sightweight = 0M;
        [DisplayName("景观修正价格")]
        public decimal? sightweight { get { return _sightweight; } set { _sightweight = value; } }
        private decimal? _frontweight = 0M;
        [DisplayName("朝向修正价格")]
        public decimal? frontweight { get { return _frontweight; } set { _frontweight = value; } }

        public int? wall { get; set; }

        public int? iselevator { get; set; }

        [DisplayName("附属房屋均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? subaverageprice { get; set; }

        public string pricedetail { get; set; }

        public int? bhousetypecode { get; set; }
        public decimal? bhousetypeweight { get; set; }
        public int? distance { get; set; }
        public decimal? distanceweight { get; set; }
        private int? _basement = 0;
        [DisplayName("地下室层数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? basement { get { return _basement; } set { _basement = value; } }

        public decimal? elevatorrateweight { get; set; }
        private int? _isyard = 0;
        public int? isyard { get { return _isyard; } set { _isyard = value; } }

        public decimal? yardweight { get; set; }

        public string doorplate { get; set; }
        public int? RightCode { get; set; }
        public int? IsVirtual { get; set; }
        public string FloorSpread { get; set; }
        public int? PodiumBuildingFloor { get; set; }

        [DisplayName("裙楼面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? PodiumBuildingArea { get; set; }

        [DisplayName("塔楼面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? TowerBuildingArea { get; set; }

        [DisplayName("地下室总面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BasementArea { get; set; }

        public string BasementPurpose { get; set; }

        [DisplayName("住宅套数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? HouseNumber { get; set; }

        [DisplayName("住宅总面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? HouseArea { get; set; }

        [DisplayName("非住宅套数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? OtherNumber { get; set; }

        [DisplayName("非住宅总面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? OtherArea { get; set; }

        public int? innerFitmentCode { get; set; }
        [DisplayName("内部装修")]
        public string innerFitmentCodeName { get; set; }

        [DisplayName("单层户数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? FloorHouseNumber { get; set; }

        [DisplayName("电梯数量")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? LiftNumber { get; set; }

        public string LiftBrand { get; set; }

        public string Facilities { get; set; }

        public int? PipelineGasCode { get; set; }

        public int? HeatingModeCode { get; set; }

        public int? WallTypeCode { get; set; }

        public double OldWeight { get; set; }

        public int? Btypecode { get; set; }

        public decimal PriceWeight { get; set; }

        public int HouseNum { get; set; }

        public string BuildingTypeName { get; set; }

        public decimal? ProAveragePrice { get; set; }

        public int? MaintenanceCode { get; set; }
        [DisplayName("维护情况")]
        public string MaintenanceCodeName { get; set; }

        [DisplayName("X坐标")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? x { get; set; }

        [DisplayName("Y坐标")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? y { get; set; }

        [DisplayName("比例尺")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? xyscale { get; set; }

        [DisplayName("备注")]
        public string remark { get; set; }
        #region 扩展
        public string creator { get; set; }
        public string saveuser { get; set; }
        public string oldid { get; set; }
        public int fxtcompanyid { get; set; }
        public string fxtcompanyname { get; set; }
        [DisplayName("数据建设机构")]
        public int belongcompanyid { get; set; }
        [DisplayName("数据建设机构")]
        public string belongcompanyname { get; set; }
        private int _valid = 1;
        public int valid { get { return _valid; } set { _valid = value; } }
        private DateTime? _createtime = DateTime.Now;
        public DateTime? createtime { get { return _createtime; } set { _createtime = value; } }
        private DateTime? _savedatetime = DateTime.Now;
        public DateTime? savedatetime { get { return _savedatetime; } set { _savedatetime = value; } }
        #endregion
    }
}
