using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_Menu
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_Menu")]
	public class SysMenu:BaseTO
	{
	
      	/// <summary>
		/// Id
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)] 
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 菜单名
        /// </summary>
        public virtual string MenuName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 父级ID
        /// </summary>
        public virtual int? ParentId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// HTTP地址
        /// </summary>
        public virtual string Url
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
        /// 菜单管理产品
        /// </summary>
        public virtual SysProductMenu ProductMenu
        {
            get;
            set;
        }
		   
	}
}