using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//Dat_House_Office_sub
		public class DatHouseOfficeSub
	{
	
      	/// <summary>
		/// 办公房号表
        /// </summary>
        public long HouseId{ get; set; }        
		/// <summary>
		/// BuildingId
        /// </summary>
        public long BuildingId{ get; set; }        
		/// <summary>
		/// ProjectId
        /// </summary>
        public long ProjectId{ get; set; }        
		/// <summary>
		/// CityId
        /// </summary>
        public int CityId{ get; set; }        
		/// <summary>
		/// 物理层
        /// </summary>
        public int FloorNo{ get; set; }        
		/// <summary>
		/// 实际层
        /// </summary>
        public string FloorNum{ get; set; }        
		/// <summary>
		/// 单元号
        /// </summary>
        public string UnitNo { get; set; }     
        /// <summary>
        /// 室号
        /// </summary>
        public string HouseNo { get; set; }   
		/// <summary>
		/// 房号名称
        /// </summary>
        public string HouseName{ get; set; }        
		/// <summary>
		/// 证载用途（1002）
        /// </summary>
        public int? PurposeCode{ get; set; }        
		/// <summary>
		/// 实际用途(1002)
        /// </summary>
        public int? SJPurposeCode{ get; set; }        
		/// <summary>
		/// 建筑面积
        /// </summary>
        public decimal? BuildingArea{ get; set; }        
		/// <summary>
		/// 套内面积(使用面积)
        /// </summary>
        public decimal? InnerBuildingArea{ get; set; }        
		/// <summary>
		/// 朝向2004
        /// </summary>
        public int? FrontCode{ get; set; }        
		/// <summary>
		/// 景观2006
        /// </summary>
        public int? SightCode{ get; set; }        
		/// <summary>
		/// 单价
        /// </summary>
        public decimal? UnitPrice{ get; set; }        
		/// <summary>
		/// 价格系数
        /// </summary>
        public decimal? Weight{ get; set; }        
		/// <summary>
		/// 是否可估
        /// </summary>
        public int? IsEValue{ get; set; }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public int FxtCompanyId{ get; set; }        
		/// <summary>
		/// 创建人ID
        /// </summary>
        public string Creator{ get; set; }        
		/// <summary>
		/// 创建时间
        /// </summary>
        public DateTime CreateTime{ get; set; }        
		/// <summary>
		/// 最后保存时间
        /// </summary>
        public DateTime? SaveDateTime{ get; set; }        
		/// <summary>
		/// 最后修改人ID
        /// </summary>
        public string SaveUser{ get; set; }        
		/// <summary>
		/// 是否有效
        /// </summary>
        public int Valid{ get; set; }        
		/// <summary>
		/// Remarks
        /// </summary>
        public string Remarks{ get; set; }        
		   
	}
}