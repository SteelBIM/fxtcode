using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//LNK_TypicalFloor_Floor
		public class LnkTypicalFloorFloor
	{
	
      	/// <summary>
		/// 标准层与楼层关联表（未用到）
        /// </summary>
        public long ID{ get; set; }        
		/// <summary>
		/// 标准层ID
        /// </summary>
        public long TypicalFloorId{ get; set; }        
		/// <summary>
		/// 楼层
        /// </summary>
        public int FloorNo{ get; set; }        
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
		   
	}
}