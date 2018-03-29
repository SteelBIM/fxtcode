using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_MenuPurview
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_MenuPurview")]
	public class SysMenuPurview:BaseTO
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
		/// 菜单Id
        /// </summary>
        public virtual int? MenuId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 权限Id
        /// </summary>
        public virtual int? PurviewId
        {
            get; 
            set; 
        }        
		   
	}
}