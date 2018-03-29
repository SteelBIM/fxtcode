using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.ComponentModel;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea_Office
    {
        /// <summary>
        /// 商务中心
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("办公商务中心ID")]
        public int SubAreaId { get; set; }
        /// <summary>
        /// AreaName
        /// </summary>
        [DisplayName("*行政区")]
        public string AreaName { get; set; }
        /// <summary>
        /// SubAreaName
        /// </summary>
        [DisplayName("*商务中心")]
        public string SubAreaName { get; set; }
        /// <summary>
        /// cityId
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("城市ID")]
        public int CityId { get; set; }
        /// <summary>
        /// AreaId
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("行政区ID")]
        public int AreaId { get; set; }
        /// <summary>
        /// AreaLine
        /// </summary>
        [DisplayName("环线")]
        public string AreaLine { get; set; }
        /// <summary>
        /// 商务中心描述
        /// </summary>
        [DisplayName("描述")]
        public string Details { get; set; }
        /// <summary>
        /// 商务中心等级1104
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("商务中心等级ID")]
        public int? TypeCode { get; set; }
        /// <summary>
        /// 商务中心等级
        /// </summary>
        [DisplayName("商务中心等级")]
        public string TypeCodeName { get; set; }
        /// <summary>
        /// X
        /// </summary>
        [DisplayName("经度")]
        public decimal? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        [DisplayName("纬度")]
        public decimal? Y { get; set; }
        /// <summary>
        /// XYScale
        /// </summary>
        [DisplayName("经纬度比例")]
        public int? XYScale { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("机构ID")]
        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("创建日期")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Creators
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("创建人")]
        public string Creators { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("修改人")]
        public string SaveUser { get; set; }
        /// <summary>
        /// SaveDate
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("修改日期")]
        public DateTime? SaveDate { get; set; }

        /// <summary>
        /// 地图标注集合
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("地图标注集合")]
        public string LngOrLat { get; set; }

    }
}
