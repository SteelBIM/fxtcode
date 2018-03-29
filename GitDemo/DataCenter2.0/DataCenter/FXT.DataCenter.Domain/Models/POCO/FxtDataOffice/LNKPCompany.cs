using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//LNK_P_Company
		public class LnkPCompany
	{
	
      	/// <summary>
		/// ID
        /// </summary>
        public long ID{ get; set; }        
		/// <summary>
		/// 楼盘公司表
        /// </summary>
        public long ProjectId{ get; set; }        
		/// <summary>
		/// 公司ID
        /// </summary>
        public long CompanyId{ get; set; }        
		/// <summary>
		/// 公司角色
        /// </summary>
        public int CompanyType{ get; set; }        
		/// <summary>
		/// CityId
        /// </summary>
        public int CityId{ get; set; }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public int FxtCompanyId{ get; set; }        
		   
	}
}