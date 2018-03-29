using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-30
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities{
		/// <summary>
 	///Sys_Purview
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_Purview")]
	public class SysPurview:BaseTO
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
		/// 权限名
        /// </summary>
        public virtual string PurviewName
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
		/// 创建时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get; 
            set; 
        }        
		   
	}
}