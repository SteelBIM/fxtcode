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
    /// 楼层表
    /// </summary>
    public class Dat_Floor_Biz
    {

        /// <summary>
        /// 楼层表
        /// </summary>
        [DisplayName("楼层ID")]
        [ExcelExportIgnore]
        public long FloorId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [DisplayName("城市")]
        [ExcelExportIgnore]
        public int CityId { get; set; }
        #region 扩展字段 城市名称
        /// <summary>
        /// 城市
        /// </summary>
        [DisplayName("城市")]
        public string CityName { get; set; }
        #endregion
        /// <summary>
        /// 楼栋ID
        /// </summary>
        [DisplayName("楼栋ID")]
        [ExcelExportIgnore]
        public long BuildingId { get; set; }

        #region 扩展字段 楼栋名称
        /// <summary>
        /// 楼栋名称
        /// </summary>
        [DisplayName("楼栋名称")]
        public string BuildingName { get; set; }
        #endregion
        /// <summary>
        /// 物理层
        /// </summary>
        [DisplayName("物理层")]
        [Required(ErrorMessage = "{0}不能为空")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        public int FloorNo { get; set; }
        /// <summary>
        /// 实际层
        /// </summary>
        [DisplayName("实际层")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string FloorNum { get; set; }
        /// <summary>
        /// 楼栋面积
        /// </summary>
        [DisplayName("楼栋面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 层高
        /// </summary>
        [DisplayName("层高")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? FloorHigh { get; set; }
        /// <summary>
        /// 楼层平面图
        /// </summary>
        [DisplayName("楼层平面图")]
        [ExcelExportIgnore]
        public string FloorPicture { get; set; }
        /// <summary>
        /// 租售方式1127
        /// </summary>
        [DisplayName("租售方式")]
        [ExcelExportIgnore]
        public int? RentSaleType { get; set; }

        #region 扩展字段 租售方式
        /// <summary>
        /// 租售方式
        /// </summary>
        [DisplayName("租售方式")]
        public string RentSaleTypeName { get; set; }
        #endregion
        /// <summary>
        /// 楼层商业类型1120
        /// </summary>
        [DisplayName("楼层商业类型")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int BuildingBizType { get; set; }

        #region 扩展字段 楼层商业类型
        /// <summary>
        /// 楼层商业类型
        /// </summary>
        [DisplayName("楼层商业类型")]
        public string BuildingBizTypeName { get; set; }
        #endregion
        /// <summary>
        /// 主营类型1120
        /// </summary>
        [DisplayName("主营类型")]
        [ExcelExportIgnore]
        public int? BizType { get; set; }
        #region 扩展字段 主营类型
        /// <summary>
        /// 主营类型
        /// </summary>
        [DisplayName("主营类型")]
        public string BizTypeName { get; set; }
        #endregion
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        [DisplayName("FxtCompanyIdID")]
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        [DisplayName("创建人ID")]
        [ExcelExportIgnore]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后保存时间
        /// </summary>
        [DisplayName("最后保存时间")]
        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// 最后修改人ID
        /// </summary>
        [DisplayName("最后修改人ID")]
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [DisplayName("是否有效")]
        [ExcelExportIgnore]
        public int Valid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("楼层均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? AveragePrice { get; set; }

        [DisplayName("价格系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? Weight { get; set; }

        #region 扩展字段
        /// <summary>
        /// 商圈Id
        /// </summary>
        [DisplayName("商圈")]
        [ExcelExportIgnore]
        public int ProjectId { get; set; }
        #endregion
    }
}
