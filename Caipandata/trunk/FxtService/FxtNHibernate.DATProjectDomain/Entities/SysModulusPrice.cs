using System;

/**
 * 作者: 李晓东
 * 时间: 2014-04-15
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///Sys_ModulusPrice
		/// </summary>
	public class SysModulusPrice
	{
	
      	/// <summary>
		/// 各种因素的系数调整
        /// </summary>
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 城市ID
        /// </summary>
        public virtual int CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总楼层开始
        /// </summary>
        public virtual int StartTotalFloor
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 总楼层结束
        /// </summary>
        public virtual int EndTotalFloor
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 所在楼层
        /// </summary>
        public virtual int WhereFloor
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 系数类型Code
        /// </summary>
        public virtual int ModulusTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 系数Code
        /// </summary>
        public virtual int ModulusCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 系数
        /// </summary>
        public virtual decimal? Modulus
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否百分比
        /// </summary>
        public virtual int Percentage
        {
            get; 
            set; 
        }        
		   
	}
}