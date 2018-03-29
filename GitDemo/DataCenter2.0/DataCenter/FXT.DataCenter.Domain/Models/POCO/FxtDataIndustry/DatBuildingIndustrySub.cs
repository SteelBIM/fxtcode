using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatBuildingIndustrySub
    {

        /// <summary>
        /// 工业楼栋表
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// BuildingName
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// 工业类型6013
        /// </summary>
        public int? IndustryType { get; set; }
        /// <summary>
        /// 用途1001
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 建筑结构2010
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// 建筑类型2003
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public int? TotalFloor { get; set; }
        /// <summary>
        /// 总高度
        /// </summary>
        public int? TotalHigh { get; set; }
        /// <summary>
        /// 总建筑面积_平方米
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 销售日期
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 租售方式1127
        /// </summary>
        public int? RentSaleType { get; set; }
        /// <summary>
        /// 客梯数量
        /// </summary>
        public int? LiftNum { get; set; }
        /// <summary>
        /// 客梯装修1140
        /// </summary>
        public int? LiftFitment { get; set; }
        /// <summary>
        /// 电梯品牌
        /// </summary>
        public string LiftBrand { get; set; }
        /// <summary>
        /// 卫浴品牌
        /// </summary>
        public string ToiletBrand { get; set; }
        /// <summary>
        /// 公共区域装修1140
        /// </summary>
        public int? PublicFitment { get; set; }
        /// <summary>
        /// 外墙装修1143
        /// </summary>
        public int? WallFitment { get; set; }
        /// <summary>
        /// 标准层层高
        /// </summary>
        public decimal? FloorHigh { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 价格系数说明
        /// </summary>
        public string PriceDetail { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public decimal? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后保存时间
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }   
    }
}
