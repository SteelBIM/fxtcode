using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//SYS_Role_User
		public class SYSRoleUser
	{
	
      	/// <summary>
		/// 角色用户对应表,用来记录角色和用户的关系
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// RoleID
        /// </summary>
        [JsonProperty(PropertyName = "roleid")]
        public virtual int RoleID
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
		/// 城市ID
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public virtual int CityID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 评估机构ID
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyID
        {
            get; 
            set; 
        }

        /// <summary>
        /// TrueName
        /// </summary>
        [JsonProperty(PropertyName = "truename")]
        public virtual string TrueName
        {
            get;
            set;
        }        
		   
	}
}