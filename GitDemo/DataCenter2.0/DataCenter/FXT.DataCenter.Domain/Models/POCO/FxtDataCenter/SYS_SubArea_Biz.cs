using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea_Biz
    {

        /// <summary>
        /// 商圈
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("商圈")]
        public int SubAreaId { get; set; }
        /// <summary>
        /// SubAreaName
        /// </summary>
        [Required(ErrorMessage="请填写商圈名称")]
        [DisplayName("商圈名称")]
        public string SubAreaName { get; set; }
        /// <summary>
        /// AreaId
        /// </summary>
        [Required(ErrorMessage="请选择行政区")]
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        /// <summary>
        /// 环线
        /// </summary>
        [DisplayName("环线")]
        public string AreaLine { get; set; }
        /// <summary>
        /// 商业繁华度描述
        /// </summary>
        [DisplayName("商业繁华度描述")]
        public string Details { get; set; }
        /// <summary>
        /// 商圈等级1103
        /// </summary>
         [DisplayName(" 商圈等级")]
         [ExcelExportIgnore]
        public int? TypeCode { get; set; }
        /// <summary>
        /// X
        /// </summary>
         [DisplayName("X坐标")]
        public decimal? x { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        [DisplayName("Y坐标")]
        public decimal? y { get; set; }
        /// <summary>
        /// XYScale
        /// </summary>
        [ExcelExportIgnore]
        public int? XYScale { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
         [ExcelExportIgnore]
        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        [ExcelExportIgnore]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Creators
        /// </summary>
         [ExcelExportIgnore]
        public string Creators { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
         [ExcelExportIgnore]
        public string SaveUser { get; set; }
        /// <summary>
        /// SaveDate
        /// </summary>
         [ExcelExportIgnore]
        public DateTime? SaveDate { get; set; }

        #region 扩展字段

        [DisplayName("行政区")]
        public string AreaName { get; set; }
        [DisplayName("商圈等级")]
        public string TypeName { get; set; }
        /// <summary>
        /// 地图标注集合
        /// </summary>
        [DisplayName("地图标注集合")]
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        #endregion 

    }
}
