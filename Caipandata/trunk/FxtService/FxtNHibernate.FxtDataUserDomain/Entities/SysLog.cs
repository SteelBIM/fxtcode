using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_Log
		/// </summary>
	public class SysLog
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
		/// 操作ID
        /// </summary>
        public virtual int OperateId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 操作表名ID
        /// </summary>
        public virtual int RelationTableId
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
		/// 操作类型 1.新增2.删除3.修改 
        /// </summary>
        public virtual int OperateType
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