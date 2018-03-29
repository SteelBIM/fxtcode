using System;

namespace FXT.DataCenter.Domain.Models.QueryObjects
{
    /// <summary>
    /// 房号信息统计Model
    /// </summary>
    public class HouseStatiParam
    {
        /// <summary>
        /// id
        /// </summary>
        public int HouseId { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public int BuildingId { get; set; }
        /// <summary>
        /// 物业名称
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public int? HouseTypeCode { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        public string UnitNo { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? BuildArea { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public int? FrontCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? SalePrice { get; set; }
        /// <summary>
        /// 权重值
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// PhotoName
        /// </summary>
        public string PhotoName { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public string OldId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid { get; set; }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// 面积确认
        /// </summary>
        public int? IsShowBuildingArea { get; set; }
        /// <summary>
        /// 套内面积
        /// </summary>
        public decimal? InnerBuildingArea { get; set; }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public int? SubHouseType { get; set; }
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public decimal? SubHouseArea { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 名义层（实际层）
        /// </summary>
        public string NominalFloor { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        public int? VDCode { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        public int? FitmentCode { get; set; }
        /// <summary>
        /// 是否有厨房
        /// </summary>
        public int? Cookroom { get; set; }
        /// <summary>
        /// 阳台数
        /// </summary>
        public int? Balcony { get; set; }
        /// <summary>
        /// 洗手间数
        /// </summary>
        public int? Toilet { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        public int? NoiseCode { get; set; }

        #region 扩展字段
        public int houseareaid { get; set; }
        public int housesubareaid { get; set; }
        public int HousePurposeCode { get; set; }
        /// <summary>
        /// 创建开始日期
        /// </summary>
        public DateTime? HstarTime { get; set; }
        /// <summary>
        /// 创建结束日期
        /// </summary>
        public DateTime? HendTime { get; set; }
        /// <summary>
        /// 系数
        /// </summary>
        public decimal? WeightTo { get; set; }
        /// <summary>
        /// 户型结构(避免与楼盘表的StructureCode字段冲突)
        /// </summary>
        public int HouseStructureCode { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int AreaId { get;set;}
        /// <summary>
        /// 片区ID
        /// </summary>
        public int SubAreaId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 房号面积
        /// 1大于
        /// 0小于
        /// </summary>
        public int BuildAreaCompany_house { get; set; }

        /// <summary>
        /// 价格系数
        /// 1大于
        /// 0小于
        /// </summary>
        public int WeightCompany_house { get; set; }

        #endregion
    }
}