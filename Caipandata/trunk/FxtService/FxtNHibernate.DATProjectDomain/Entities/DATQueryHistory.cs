using System;

/**
 * 作者: 李晓东
 * 时间: 2014-04-10
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///DAT_QueryHistory
		/// </summary>
	public class DATQueryHistory
	{
	
      	/// <summary>
		/// Id
        /// </summary>
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CityId
        /// </summary>
        public virtual int CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UserId
        /// </summary>
        public virtual string UserId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// BuildingId
        /// </summary>
        public virtual int? BuildingId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// HouseId
        /// </summary>
        public virtual int? HouseId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ProjectName
        /// </summary>
        public virtual string ProjectName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// BuildingName
        /// </summary>
        public virtual string BuildingName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// HouseName
        /// </summary>
        public virtual string HouseName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 询价时间
        /// </summary>
        public virtual DateTime? QueryDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UnitPrice
        /// </summary>
        public virtual decimal? UnitPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// TotalPrice
        /// </summary>
        public virtual decimal? TotalPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 税费总额
        /// </summary>
        public virtual decimal? tax
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 净值
        /// </summary>
        public virtual decimal? newprice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 原购价
        /// </summary>
        public virtual decimal? oldprice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// BuildingArea
        /// </summary>
        public virtual decimal? BuildingArea
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 数据类型
        /// </summary>
        public virtual int type
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例开始时间
        /// </summary>
        public virtual DateTime? CaseStartDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例结束时间
        /// </summary>
        public virtual DateTime? CaseEndDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例数
        /// </summary>
        public virtual int? CaseNumber
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例最大值
        /// </summary>
        public virtual int? CaseMaxPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例最小值
        /// </summary>
        public virtual int? CaseMInPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 项目均价
        /// </summary>
        public virtual decimal? EPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 案例均价
        /// </summary>
        public virtual int? CaseAvgPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋均价
        /// </summary>
        public virtual decimal? BEPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 房号估价
        /// </summary>
        public virtual decimal? HEPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// IPAddress
        /// </summary>
        public virtual string IPAddress
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 客户单位ID
        /// </summary>
        public virtual int? CompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 评估机构ID
        /// </summary>
        public virtual int FxtCompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FileNo
        /// </summary>
        public virtual string FileNo
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 成交价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 税费条件选择:非普通住宅，企业，满5年，生活唯一用房，首次购房
        /// </summary>
        public virtual string TaxSelect
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 税费明细
        /// </summary>
        public virtual string TaxDetali
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 询价目的
        /// </summary>
        public virtual int? QueryPurposeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 市场比较法案例1
        /// </summary>
        public virtual int? CaseId1
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 市场比较法案例2
        /// </summary>
        public virtual int? CaseId2
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 市场比较法案例3
        /// </summary>
        public virtual int? CaseId3
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 比价平台ID
        /// </summary>
        public virtual string CASId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Qid
        /// </summary>
        public virtual int? Qid
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 附属房屋类型
        /// </summary>
        public virtual int? SubHouseType
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 附属房屋面积
        /// </summary>
        public virtual decimal? SubHouseArea
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 附属房屋单价
        /// </summary>
        public virtual decimal? SubHouseUnitPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 附属房屋总价
        /// </summary>
        public virtual decimal? SubHouseTotalPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DiscountUintPrice
        /// </summary>
        public virtual decimal? DiscountUintPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DiscountTotalPrice
        /// </summary>
        public virtual decimal? DiscountTotalPrice
        {
            get; 
            set; 
        }        
		   
	}
}