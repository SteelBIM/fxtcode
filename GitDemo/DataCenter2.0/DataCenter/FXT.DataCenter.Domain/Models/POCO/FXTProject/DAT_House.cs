using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_House
    {
        [ExcelExportIgnore]
        public string oldid { get; set; }
        [ExcelExportIgnore]
        public int cityid { get; set; }
        [ExcelExportIgnore]
        public int houseid { get; set; }
        [ExcelExportIgnore]
        public int buildingid { get; set; }

        [DisplayName("*行政区")]
        public string AreaName { get; set; }
        [DisplayName("*楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("*楼栋名称")]
        public string BuildingName { get; set; }

        [DisplayName("*房号名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string housename { get; set; }

        [DisplayName("*所在楼层")]
        public int floorno { get; set; }

        [DisplayName("*单元室号")]
        public string unitno { get; set; }

        [DisplayName("实际层")]
        public string nominalfloor { get; set; }

        [DisplayName("建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? buildarea { get; set; }

        [DisplayName("套内面积")]
        public decimal? innerbuildingarea { get; set; }

        [ExcelExportIgnore]
        public int? subhousetype { get; set; }
        [DisplayName("附属房屋类型")]
        public string SubHouseTypeName { get; set; }
        [DisplayName("附属房屋面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? subhousearea { get; set; }

        [ExcelExportIgnore]
        public int? housetypecode { get; set; }
        [DisplayName("户型")]
        public string HouseTypeCodeName { get; set; }

        [ExcelExportIgnore]
        public int? structurecode { get; set; }
        [DisplayName("户型结构")]
        public string StructureCodeName { get; set; }

        [DisplayName("单价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? unitprice { get; set; }

        [ExcelExportIgnore]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? saleprice { get; set; }

        [DisplayName("总价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? totalprice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? weight { get; set; }

        [ExcelExportIgnore]
        public int? purposecode { get; set; }
        [DisplayName("用途")]
        public string PurposeCodeName { get; set; }

        [ExcelExportIgnore]
        public int? frontcode { get; set; }
        [DisplayName("朝向")]
        public string FrontCodeName { get; set; }

        [ExcelExportIgnore]
        public int? sightcode { get; set; }
        [DisplayName("景观")]
        public string SightCodeName { get; set; }

        [ExcelExportIgnore]
        public int? VDCode { get; set; }
        [DisplayName("通风采光")]
        public string VDCodeName { get; set; }

        [ExcelExportIgnore]
        public int? FitmentCode { get; set; }
        [DisplayName("装修")]
        public string FitmentCodeName { get; set; }

        [ExcelExportIgnore]
        public int? Cookroom { get; set; }
        [DisplayName("是否有厨房")]
        public string CookroomName { get; set; }

        [DisplayName("阳台数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        public int? Balcony { get; set; }

        [DisplayName("洗手间数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        public int? Toilet { get; set; }

        [ExcelExportIgnore]
        public int? isshowbuildingarea { get; set; }
        [DisplayName("面积确认")]
        public string IsShowBuildingAreaName { get; set; }

        [ExcelExportIgnore]
        public int? NoiseCode { get; set; }
        [DisplayName("噪音情况")]
        public string NoiseCodeName { get; set; }

        [ExcelExportIgnore]
        public int? isevalue { get; set; }
        [DisplayName("是否可估")]
        public string IsEValueName { get; set; }

        [DisplayName("备注")]
        public string remark { get; set; }

        [ExcelExportIgnore]
        public string photoname { get; set; }

        private int _valid = 1;
        [ExcelExportIgnore]
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        [ExcelExportIgnore]
        public DateTime createtime { get; set; }
        [ExcelExportIgnore]
        public string creator { get; set; }
        [ExcelExportIgnore]
        public DateTime savedatetime { get; set; }
        [ExcelExportIgnore]
        public string saveuser { get; set; }
        [ExcelExportIgnore]
        public string ProjectCreator { get; set; }
        [ExcelExportIgnore]
        public string BuildingCreator { get; set; }
        [ExcelExportIgnore]
        public int fxtcompanyid { get; set; }
        [DisplayName("数据建设机构")]
        [ExcelExportIgnore]
        public int belongcompanyid { get; set; }
        [ExcelExportIgnore]
        public string fxtcompanyname { get; set; }
        [DisplayName("数据建设机构")]
        public string belongcompanyname { get; set; }
        [ExcelExportIgnore]
        public List<DAT_House> ListUnitNo { get; set; }
        [ExcelExportIgnore]
        public int totalFloor { get; set; }
    }
}
