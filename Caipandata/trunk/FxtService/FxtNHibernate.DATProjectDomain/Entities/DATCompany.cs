using System;

/**
 * 作者: 李晓东
 * 时间: 2014-03-21
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///DAT_Company
		/// </summary>
	public class DATCompany
	{
	
      	/// <summary>
		/// CompanyId
        /// </summary>
        public virtual int CompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ChineseName
        /// </summary>
        public virtual string ChineseName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// EnglishName
        /// </summary>
        public virtual string EnglishName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CompanyTypeCode
        /// </summary>
        public virtual int? CompanyTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CityId
        /// </summary>
        public virtual int? CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Address
        /// </summary>
        public virtual string Address
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Telephone
        /// </summary>
        public virtual string Telephone
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Fax
        /// </summary>
        public virtual string Fax
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Website
        /// </summary>
        public virtual string Website
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateDate
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Valid
        /// </summary>
        public virtual int? Valid
        {
            get; 
            set; 
        }        
		   
	}
}