using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//SYS_Role_Menu_Function
		public class SYSRoleMenuFunction
	{
	
      	/// <summary>
		/// 角色菜单(页面)功能权限表，记录角色在某个菜单页面上的功能权限
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 角色菜单(页面)ID
        /// </summary>
        [JsonProperty(PropertyName = "rolemenuid")]
        public virtual int RoleMenuID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 功能CODE,1201
        /// </summary>
        [JsonProperty(PropertyName = "functioncode")]
        public virtual int FunctionCode
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
		   
	}
}