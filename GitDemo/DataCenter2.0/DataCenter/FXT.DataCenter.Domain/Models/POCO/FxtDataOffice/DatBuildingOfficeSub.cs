using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//Dat_Building_Office_sub
		public class DatBuildingOfficeSub
	{
	
      	/// <summary>
		/// 办公楼栋表
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
		/// BuildingName
        /// </summary>
        public string BuildingName{ get; set; }        
		/// <summary>
		/// OtherName
        /// </summary>
        public string OtherName{ get; set; }        
		/// <summary>
		/// 写字楼等级
        /// </summary>
        public int? OfficeType{ get; set; }        
		/// <summary>
		/// 用途
        /// </summary>
        public int? PurposeCode{ get; set; }        
		/// <summary>
		/// 建筑结构2010
        /// </summary>
        public int? StructureCode{ get; set; }        
		/// <summary>
		/// 建筑类型2003
        /// </summary>
        public int? BuildingTypeCode{ get; set; }        
		/// <summary>
		/// 总楼层
        /// </summary>
        public int? TotalFloor{ get; set; }        
		/// <summary>
		/// 总高度
        /// </summary>
        public int? TotalHigh{ get; set; }        
		/// <summary>
		/// 总建筑面积
        /// </summary>
        public decimal? BuildingArea{ get; set; }        
		/// <summary>
		/// 竣工日期
        /// </summary>
        public DateTime? EndDate{ get; set; }        
		/// <summary>
		/// 销售日期
        /// </summary>
        public DateTime? SaleDate{ get; set; }        
		/// <summary>
		/// 租售方式1127
        /// </summary>
        public int? RentSaleType{ get; set; }        
		/// <summary>
		/// 办公面积
        /// </summary>
        public decimal? OfficeArea{ get; set; }        
		/// <summary>
		/// 办公总层数
        /// </summary>
        public int? OfficeFloor{ get; set; }        
		/// <summary>
		/// 裙楼层数
        /// </summary>
        public int? PodiumBuildingNum{ get; set; }        
		/// <summary>
		/// 地下室层数
        /// </summary>
        public int? BasementNum{ get; set; }        
		/// <summary>
		/// 功能分布：裙楼、塔楼、地下室
        /// </summary>
        public string Functional{ get; set; }        
		/// <summary>
		/// 大堂面积
        /// </summary>
        public decimal? LobbyArea{ get; set; }        
		/// <summary>
		/// 大堂层高
        /// </summary>
        public decimal? LobbyHigh{ get; set; }        
		/// <summary>
		/// 大堂装修1140
        /// </summary>
        public int? LobbyFitment{ get; set; }        
		/// <summary>
		/// 客梯数量
        /// </summary>
        public int? LiftNum{ get; set; }        
		/// <summary>
		/// 客梯装修1140
        /// </summary>
        public int? LiftFitment{ get; set; }        
		/// <summary>
		/// 电梯品牌
        /// </summary>
        public string LiftBrand{ get; set; }        
		/// <summary>
		/// 卫浴品牌
        /// </summary>
        public string ToiletBrand{ get; set; }        
		/// <summary>
		/// 公共区域装修1140
        /// </summary>
        public int? PublicFitment{ get; set; }        
		/// <summary>
		/// 外墙装修1143
        /// </summary>
        public int? WallFitment{ get; set; }        
		/// <summary>
		/// 标准层层高
        /// </summary>
        public decimal? FloorHigh{ get; set; }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public int FxtCompanyId{ get; set; }        
		/// <summary>
		/// X
        /// </summary>
        public decimal? X{ get; set; }        
		/// <summary>
		/// Y
        /// </summary>
        public decimal? Y{ get; set; }        
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