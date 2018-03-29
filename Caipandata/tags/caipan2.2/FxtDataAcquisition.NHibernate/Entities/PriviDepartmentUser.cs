using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//Privi_Department_User
		public class PriviDepartmentUser
	{
	
      	/// <summary>
		/// ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// DepartmentID
        /// </summary>
        [JsonProperty(PropertyName = "departmentid")]
        public virtual int DepartmentID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UserName
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public virtual string UserName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CityID
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public virtual int CityID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// FxtCompanyID
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateDate
        /// </summary>
        [JsonProperty(PropertyName = "createdate")]
        public virtual DateTime? CreateDate
        {
            get; 
            set; 
        }        
		   
	}
}