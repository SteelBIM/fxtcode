
using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_ProductMenu
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_ProductMenu")]
	public class SysProductMenu:BaseTO
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
		/// 所属产品Id
        /// </summary>
        public virtual int ProductId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 所属菜单Id
        /// </summary>
        public virtual int MenuId
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