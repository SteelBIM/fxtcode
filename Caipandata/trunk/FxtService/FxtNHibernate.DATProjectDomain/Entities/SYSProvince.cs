using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-19
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///SYS_Province
		/// </summary>
	public class SYSProvince
	{
	
      	/// <summary>
		/// ProvinceId
        /// </summary>
        public virtual int ProvinceId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 省的名称
        /// </summary>
        public virtual string ProvinceName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 别名
        /// </summary>
        public virtual string Alias
        {
            get; 
            set; 
        }        
		/// <summary>
		/// GIS_ID
        /// </summary>
        public virtual int? GIS_ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// OldId
        /// </summary>
        public virtual int? OldId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// X坐标
        /// </summary>
        public virtual decimal? X
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Y坐标
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
		/// 是否直辖市
        /// </summary>
        public virtual int? IsZXS
        {
            get; 
            set; 
        }        
		   
	}
}