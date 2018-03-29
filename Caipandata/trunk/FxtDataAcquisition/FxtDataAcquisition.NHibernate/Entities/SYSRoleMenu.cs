using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//SYS_Role_Menu
		public class SYSRoleMenu
	{
	
      	/// <summary>
		/// 角色菜单(页面)对应表
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 角色ID
        /// </summary>
        [JsonProperty(PropertyName = "roleid")]
        public virtual int RoleID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 菜单(页面)ID
        /// </summary>
        [JsonProperty(PropertyName = "menuid")]
        public virtual int MenuID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 城市ID,当为0时表示公共
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public virtual int CityID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 评估机构ID,当为0时表示公共
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyID
        {
            get; 
            set; 
        }        
		   
	}
}