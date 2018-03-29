using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-06-09
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities{
		/// <summary>
 	///Sys_CityFxtCompany
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_CityFxtCompany")]
	public class SysCityFxtCompany:BaseTO
	{
	
      	/// <summary>
		/// 每个城市指定的评估机构
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 城市
        /// </summary>
        public virtual int CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 贷后运营评估机构
        /// </summary>
        public virtual int FxtCompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 贷后数据评估机构
        /// </summary>
        public virtual int CompanyId
        {
            get; 
            set; 
        }        
		   
	}
}