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
    /// 土地公共类
    /// </summary>
    public class LandCommon
    {
        #region 扩展字段

        /// <summary>
        /// 评估机构名称
        /// </summary>
        [DisplayName("评估机构名称")]
        [ExcelExportIgnore]
        public string CompanyName { get; set; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        [DisplayName("行政区名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string AreaName { get; set; }

        /// <summary>
        /// 片区名称
        /// </summary>
        [DisplayName("片区名称")]
        public string SubAreaName { get; set; }

        /// <summary>
        /// 地图标注集合
        /// </summary>
        [DisplayName("地图标注集合")]
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }
        #endregion
        /// <summary>
        /// 土地等级code名称
        /// </summary>
        [DisplayName("土地等级名称")]
        public string LandClassName { get; set; }

        /// <summary>
        /// 土地用途code名称
        /// </summary>
        [DisplayName("土地用途名称")]
        public string LandPurposeName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [DisplayName("城市名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string CityName { get; set; }

        /// <summary>
        /// 土地用途集合
        /// </summary>
        [DisplayName("土地用途集合")]
        [ExcelExportIgnore]
        public string opValue { get; set; }

        /// <summary>
        /// 查看命令 
        /// self 查看自己
        /// all 查看所有
        /// </summary>
        [DisplayName("查看命令")]
        [ExcelExportIgnore]
        public bool command { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        [DisplayName("页索引")]
        [ExcelExportIgnore]
        public int pageIndex { get; set; }

        /// <summary>
        /// 地价公布结束时间
        /// </summary>
        [DisplayName("地价公布结束时间")]
        [ExcelExportIgnore]
        public DateTime? priceenddate { get; set; }



        /// <summary>
        /// 土地基础信息表
        /// 土地所有者
        /// </summary>
        [DisplayName("土地所有者")]
        [ExcelExportIgnore]
        public string LandOwnerName { get; set; }

        /// <summary>
        /// 土地基础信息表
        /// 土地使用者
        /// </summary>
        [DisplayName("土地使用者")]
        [ExcelExportIgnore]
        public string LandUseName { get; set; }
    }
}
