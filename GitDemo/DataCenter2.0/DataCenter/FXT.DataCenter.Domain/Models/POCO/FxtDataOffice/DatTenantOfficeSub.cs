using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//Dat_Tenant_Office_sub
		public class DatTenantOfficeSub
	{
	
      	/// <summary>
		/// 房号租客表
        /// </summary>
        public long HouseTenantId{ get; set; }        
		/// <summary>
		/// CityId
        /// </summary>
        public int CityId{ get; set; }        
		/// <summary>
		/// ProjectId
        /// </summary>
        public long ProjectId{ get; set; }        
		/// <summary>
		/// BuildingId
        /// </summary>
        public long BuildingId{ get; set; }        
		/// <summary>
		/// HouseId
        /// </summary>
        public long HouseId{ get; set; }        
		/// <summary>
		/// 是否空置
        /// </summary>
        public int IsVacant{ get; set; }        
		/// <summary>
		/// 租赁面积
        /// </summary>
        public decimal? BuildingArea{ get; set; }        
		/// <summary>
		/// 租金单价（元月/平方米）
        /// </summary>
        public decimal? Rent{ get; set; }        
		/// <summary>
		/// 租客（商家）CompanyId
        /// </summary>
        public long TenantID{ get; set; }        
		/// <summary>
		/// 行业大类1158
        /// </summary>
        public int TypeCode{ get; set; }        
		/// <summary>
		/// 行业小类1159~1177
        /// </summary>
        public int? SubTypeCode{ get; set; }        
		/// <summary>
		/// 进驻时间
        /// </summary>
        public DateTime? JoinDate{ get; set; }        
		/// <summary>
		/// 调查时间
        /// </summary>
        public DateTime? SurveyDate{ get; set; }        
		/// <summary>
		/// 调查人
        /// </summary>
        public string SurveyUser{ get; set; }        
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
		/// 是否有效
        /// </summary>
        public int Valid{ get; set; }        
		/// <summary>
		/// 最后保存时间
        /// </summary>
        public DateTime? SaveDateTime{ get; set; }        
		/// <summary>
		/// 最后修改人ID
        /// </summary>
        public string SaveUser{ get; set; }        
		/// <summary>
		/// Remarks
        /// </summary>
        public string Remarks{ get; set; }        
		/// <summary>
		/// 是否典型租客
        /// </summary>
        public int? IsTypical{ get; set; }        
		   
	}
}