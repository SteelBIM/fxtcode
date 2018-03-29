using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_Product
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_Product")]
	public class SysProduct:BaseTO
	{
	
      	/// <summary>
		/// 自增
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)] 
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 产品名称
        /// </summary>
        public virtual string ProductName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 创建者Id
        /// </summary>
        public virtual int UserId
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