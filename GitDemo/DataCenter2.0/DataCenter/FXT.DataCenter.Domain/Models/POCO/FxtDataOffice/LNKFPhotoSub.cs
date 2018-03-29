using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models{
	 	//LNK_F_Photo_Sub
		public class LnkFPhotoSub
	{
	
      	/// <summary>
		/// 楼层图片
        /// </summary>
        public long Id{ get; set; }        
		/// <summary>
		/// 楼层ID
        /// </summary>
        public long FloorId{ get; set; }        
		/// <summary>
		/// 图片类型1181
        /// </summary>
        public int PhotoTypeCode{ get; set; }        
		/// <summary>
		/// 图片路径
        /// </summary>
        public string Path{ get; set; }        
		/// <summary>
		/// 时间
        /// </summary>
        public DateTime? PhotoDate{ get; set; }        
		/// <summary>
		/// 图片名称
        /// </summary>
        public string PhotoName{ get; set; }        
		/// <summary>
		/// CityId
        /// </summary>
        public int CityId{ get; set; }        
		/// <summary>
		/// Valid
        /// </summary>
        public int Valid{ get; set; }        
		/// <summary>
		/// FxtCompanyId
        /// </summary>
        public int FxtCompanyId{ get; set; }        
		/// <summary>
		/// 最后修改人
        /// </summary>
        public string SaveUser{ get; set; }        
		/// <summary>
		/// 最后修改时间
        /// </summary>
        public DateTime? SaveDate{ get; set; }        
		   
	}
}