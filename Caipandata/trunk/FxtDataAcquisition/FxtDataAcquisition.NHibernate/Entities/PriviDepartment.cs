using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//Privi_Department
		public class PriviDepartment
	{
	
      	/// <summary>
		/// DepartmentId
        /// </summary>
        [JsonProperty(PropertyName = "departmentid")]
        public virtual int DepartmentId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Fk_CompanyId
        /// </summary>
        [JsonProperty(PropertyName = "fk_companyid")]
        public virtual int Fk_CompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DepartmentName
        /// </summary>
        [JsonProperty(PropertyName = "departmentname")]
        public virtual string DepartmentName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FK_CityId
        /// </summary>
        [JsonProperty(PropertyName = "fk_cityid")]
        public virtual int FK_CityId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FK_DepTypeCode
        /// </summary>
        [JsonProperty(PropertyName = "fk_deptypecode")]
        public virtual int FK_DepTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FK_ParentId
        /// </summary>
        [JsonProperty(PropertyName = "fk_parentid")]
        public virtual int? FK_ParentId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Address
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public virtual string Address
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 传真
        /// </summary>
        [JsonProperty(PropertyName = "fax")]
        public virtual string Fax
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 电话
        /// </summary>
        [JsonProperty(PropertyName = "telephone")]
        public virtual string Telephone
        {
            get; 
            set; 
        }        
		/// <summary>
		/// EMail
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public virtual string EMail
        {
            get; 
            set; 
        }        
		/// <summary>
		/// LinkMan
        /// </summary>
        [JsonProperty(PropertyName = "linkman")]
        public virtual string LinkMan
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 0公共
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int? FxtCompanyId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DValid
        /// </summary>
        [JsonProperty(PropertyName = "dvalid")]
        public virtual int? DValid
        {
            get; 
            set; 
        }
        /// <summary>
        [JsonProperty(PropertyName = "fk_depattr")]
		/// 部门属性：直属部门，分支机构
        /// </summary>
        public virtual int? FK_DepAttr
        {
            get; 
            set; 
        }        
		   
	}
}