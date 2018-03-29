using System;

/**
 * 作者: 李晓东
 * 时间: 2014-04-15
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///Sys_FloorPrice
		/// </summary>
	public class SysFloorPrice
	{
	
      	/// <summary>
		/// id
        /// </summary>
        public virtual int id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CityId
        /// </summary>
        public virtual int? CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总楼层开始
        /// </summary>
        public virtual int? 总楼层开始
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总楼层结束
        /// </summary>
        public virtual int? 总楼层结束
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 所在楼层
        /// </summary>
        public virtual int? 所在楼层
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋均价开始
        /// </summary>
        public virtual int? 楼栋均价开始
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼栋均价结束
        /// </summary>
        public virtual int? 楼栋均价结束
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 楼层差
        /// </summary>
        public virtual decimal? 楼层差
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否带电梯
        /// </summary>
        public virtual int? 是否带电梯
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 均价层
        /// </summary>
        public virtual string 均价层
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否百分比
        /// </summary>
        public virtual int? 是否百分比
        {
            get; 
            set; 
        }        
		   
	}
}