using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 基准地价表
    /// </summary>
    public class DAT_Land_BasePrice
    {
        public DAT_Land_BasePrice()
        {
            this.pageIndex = 1;
            this.pageSize = 30;
        }
        [DisplayName("土地基准地价ID")]
        [ExcelExportIgnore]
        public long id { get; set; }

        [ExcelExportIgnore]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int cityid { get; set; }
        [DisplayName("城市")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string CityName { get; set; }

        [ExcelExportIgnore]
        public int areaid { get; set; }
        [DisplayName("行政区")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string AreaName { get; set; }

        [ExcelExportIgnore]
        public int subareaid { get; set; }
        [DisplayName("片区")]
        public string SubAreaName { get; set; }

        [Required(ErrorMessage = "土地用途不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择土地用途")]
        [DisplayName("土地用途")]
        [ExcelExportIgnore]
        public int purposecode { get; set; }
        [DisplayName("*土地用途")]
        public string PurposeCodeName { get; set; }

        [DisplayName("土地等级")]
        [Required(ErrorMessage = "土地等级不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择土地等级")]
        [ExcelExportIgnore]
        public int landclass { get; set; }
        [DisplayName("*土地等级")]
        public string LandClassName { get; set; }

        [DisplayName("土地平均单价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "土地平均单价必须是数字类型")]
        public decimal? landunitprice_avg { get; set; }

        [DisplayName("土地最低单价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "土地最低单价必须是数字类型")]
        public decimal? landunitprice_min { get; set; }

        [DisplayName("土地最高单价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "土地最高单价必须是数字类型")]
        public decimal? landunitprice_max { get; set; }

        [DisplayName("建面平均地价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "建面平均地价必须是数字类型")]
        public decimal? buildingunitprice_avg { get; set; }

        [DisplayName("建面最低地价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "建面最低地价必须是数字类型")]
        public decimal? buildingunitprice_min { get; set; }

        [DisplayName("建面最高地价_元/平方米")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "建面最高地价必须是数字类型")]
        public decimal? buildingunitprice_max { get; set; }

        [DisplayName("*地价公布日期")]
        [Required(ErrorMessage = "地价公布日期不能为空")]
        public DateTime? pricedate { get; set; }

        [DisplayName("发布公文号")]
        public string DocumentNo { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        #region 扩展字段
        [ExcelExportIgnore]
        public int fxtcompanyid { get; set; }
        [ExcelExportIgnore]
        public int valid { get; set; }
        [DisplayName("地图标注集合")]
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }
        [DisplayName("土地用途集合")]
        [ExcelExportIgnore]
        public string opValue { get; set; }

        //[DisplayName("查看命令")]
        //[ExcelExportIgnore]
        //public bool command { get; set; }

        [DisplayName("页索引")]
        [ExcelExportIgnore]
        public int pageIndex { get; set; }
        [DisplayName("页大小")]
        [ExcelExportIgnore]
        public int pageSize { get; set; }
        [DisplayName("地价公布结束时间")]
        [ExcelExportIgnore]
        public DateTime? priceenddate { get; set; }

        //[DisplayName("土地所有者")]
        //[ExcelExportIgnore]
        //public string LandOwnerName { get; set; }

        //[DisplayName("土地使用者")]
        //[ExcelExportIgnore]
        //public string LandUseName { get; set; }
        #endregion
    }
}
