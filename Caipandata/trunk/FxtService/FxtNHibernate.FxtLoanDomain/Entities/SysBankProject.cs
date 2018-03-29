using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-06-09
 * 摘要: 新建实体类
 *          新增:贺黎亮  2014.06.11 添加Valid属性
 * 
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities{
		/// <summary>
 	///Sys_Project
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_BankProject")]
	public class SysBankProject:BaseTO
	{
	
      	/// <summary>
		/// 2014.06.06 新增文件项目表
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 银行ID
        /// </summary>
        public virtual int BankId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 项目名称
        /// </summary>
        public virtual string ProjectName
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
		/// 所属用户
        /// </summary>
        public virtual int UserId
        {
            get; 
            set; 
        }
  
        /// <summary>
        /// 是否删除 1正常  0 已删除
        /// </summary>
        public virtual int Valid
        {
            get;
            set;
        }
        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }

	}
}