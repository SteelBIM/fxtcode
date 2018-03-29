using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-02-20
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities{
		/// <summary>
 	///Data_Building
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Data_Building")]
    public class DataBuilding : BaseTO
	{
	
      	/// <summary>
		/// ID
        /// </summary>
        [SQLField("BuildingId", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int BuildingId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼宇名称
        /// </summary>
        public virtual string BuildingName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼盘ID
        /// </summary>
        public virtual int ProjectId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼宇用途
        /// </summary>
        public virtual int? PurposeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑结构
        /// </summary>
        public virtual int? StructureCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑类型
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总层数
        /// </summary>
        public virtual int? TotalFloor
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 层高
        /// </summary>
        public virtual decimal? FloorHigh
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 销售许可证
        /// </summary>
        public virtual string SaleLicence
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 梯户比
        /// </summary>
        public virtual string ElevatorRate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 单元数
        /// </summary>
        public virtual int? UnitsNumber
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总户数
        /// </summary>
        public virtual int? TotalNumber
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑面积
        /// </summary>
        public virtual decimal? TotalBuildArea
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑时间
        /// </summary>
        public virtual DateTime? BuildDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 销售时间
        /// </summary>
        public virtual DateTime? SaleDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 销售均价
        /// </summary>
        public virtual decimal? AveragePrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// AverageFloor
        /// </summary>
        public virtual int? AverageFloor
        {
            get; 
            set; 
        }        
		/// <summary>
		/// JoinDate
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// LicenceDate
        /// </summary>
        public virtual DateTime? LicenceDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋别名
        /// </summary>
        public virtual string OtherName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 权重值
        /// </summary>
        public virtual decimal? Weight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否可估
        /// </summary>
        public virtual int? IsEValue
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CityID
        /// </summary>
        public virtual int CityID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get; 
            set; 
        }        
		/// <summary>
		/// OldId
        /// </summary>
        public virtual string OldId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Valid
        /// </summary>
        public virtual int? Valid
        {
            get; 
            set; 
        }        
		/// <summary>
		/// SalePrice
        /// </summary>
        public virtual decimal? SalePrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// SaveDateTime
        /// </summary>
        public virtual DateTime? SaveDateTime
        {
            get; 
            set; 
        }        
		/// <summary>
		/// save user
        /// </summary>
        public virtual string SaveUser
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋位置
        /// </summary>
        public virtual int? LocationCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋景观
        /// </summary>
        public virtual int? SightCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋朝向
        /// </summary>
        public virtual int? FrontCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋结构修正价格
        /// </summary>
        public virtual decimal? StructureWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑类型修正价格
        /// </summary>
        public virtual decimal? BuildingTypeWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 年期修正价格
        /// </summary>
        public virtual decimal? YearWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 用途修正价格
        /// </summary>
        public virtual decimal? PurposeWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋位置修正价格
        /// </summary>
        public virtual decimal? LocationWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 景观修正价格
        /// </summary>
        public virtual decimal? SightWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 朝向修正价格
        /// </summary>
        public virtual decimal? FrontWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public virtual int? FxtCompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// X
        /// </summary>
        public virtual decimal? X
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Y
        /// </summary>
        public virtual decimal? Y
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 外墙
        /// </summary>
        public virtual int? Wall
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否带电梯
        /// </summary>
        public virtual int? IsElevator
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 附属房屋均价
        /// </summary>
        public virtual decimal? SubAveragePrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 价格系数说明
        /// </summary>
        public virtual string PriceDetail
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋户型面积修正因素
        /// </summary>
        public virtual int? BHouseTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋户型面积修正价格
        /// </summary>
        public virtual decimal? BHouseTypeWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Creator
        /// </summary>
        public virtual string Creator
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Distance
        /// </summary>
        public virtual int? Distance
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DistanceWeight
        /// </summary>
        public virtual decimal? DistanceWeight
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 地下室层数
        /// </summary>
        public virtual int? basement
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Remark
        /// </summary>
        public virtual string Remark
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Status
        /// </summary>
        public virtual int? Status
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FxtBuildingId
        /// </summary>
        public virtual int? FxtBuildingId
        {
            get; 
            set; 
        }        
		   
	}
}