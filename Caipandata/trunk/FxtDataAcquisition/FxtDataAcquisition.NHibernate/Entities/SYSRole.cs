using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//SYS_Role
		public class SYSRole
	{
	
      	/// <summary>
		/// 角色表
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 角色名称
        /// </summary>
        [JsonProperty(PropertyName = "rolename")]
        public virtual string RoleName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Remarks
        /// </summary>
        [JsonProperty(PropertyName = "remarks")]
        public virtual string Remarks
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Valid
        /// </summary>
        [JsonProperty(PropertyName = "valid")]
        public virtual int Valid
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateTime
        /// </summary>
        [JsonProperty(PropertyName = "createtime")]
        public virtual DateTime? CreateTime
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 城市ID，0表示公共
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public virtual int CityID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 评估机构ID，0表示公共
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyID
        {
            get; 
            set; 
        }        
		   
	}
}