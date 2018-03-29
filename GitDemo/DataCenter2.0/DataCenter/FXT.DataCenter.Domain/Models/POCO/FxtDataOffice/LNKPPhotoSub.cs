using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
	 	//LNK_P_Photo_Sub
		public class LnkPPhotoSub
	{
	
      	/// <summary>
		/// Id
        /// </summary>
        public long Id{ get; set; }        
		/// <summary>
		/// ProjectId
        /// </summary>
        public long ProjectId{ get; set; }        
		/// <summary>
		/// PhotoTypeCode
        /// </summary>
        public int PhotoTypeCode{ get; set; }        
		/// <summary>
		/// Path
        /// </summary>
        public string Path{ get; set; }        
		/// <summary>
		/// PhotoDate
        /// </summary>
        public DateTime? PhotoDate{ get; set; }        
		/// <summary>
		/// PhotoName
        /// </summary>
        public string PhotoName{ get; set; }        
		/// <summary>
		/// CityId
        /// </summary>
        public int CityId{ get; set; }        
		/// <summary>
		/// Valid
        /// </summary>
        public int? Valid{ get; set; }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public int FxtCompanyId{ get; set; }        
		/// <summary>
		/// SaveUser
        /// </summary>
        public string SaveUser{ get; set; }        
		/// <summary>
		/// SaveDate
        /// </summary>
        public DateTime? SaveDate{ get; set; }        
		   
	}
}