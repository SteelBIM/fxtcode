using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_Relation
		/// </summary>
	public class SysRelation
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
		/// 用户ID
        /// </summary>
        public virtual int UserId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 关联ID
        /// </summary>
        public virtual int RelationId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 关联表名ID
        /// </summary>
        public virtual int RelationTableId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 创建时间
        /// </summary>
        public virtual DateTime CreateDate
        {
            get; 
            set; 
        }        
		   
	}
}