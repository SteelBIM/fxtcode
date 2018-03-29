using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_UserProduct
		/// </summary>
	public class SysUserProduct
	{
	
      	/// <summary>
		/// 自增
        /// </summary>
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 用户Id
        /// </summary>
        public virtual string UserId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 产品Id
        /// </summary>
        public virtual int ProductId
        {
            get; 
            set; 
        }
        /// <summary>
        /// 有效日期
        /// </summary>
        public virtual DateTime? ExpiredDate
        {
            get;
            set;
        }  
		/// <summary>
		/// 创建时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 备注
        /// </summary>
        public virtual string Remark
        {
            get; 
            set; 
        }        
		   
	}
}