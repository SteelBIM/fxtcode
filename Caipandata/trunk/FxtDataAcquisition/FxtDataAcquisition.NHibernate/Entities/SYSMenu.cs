using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//SYS_Menu
		public class SYSMenu
	{
	
      	/// <summary>
		/// 菜单(页面)表，用来记录菜单名称、子菜单、页面名称
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 父级菜单ID
        /// </summary>
        [JsonProperty(PropertyName = "parentid")]
        public virtual int ParentID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "menuname")]
        public virtual string MenuName
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
		/// Remark
        /// </summary>
        [JsonProperty(PropertyName = "remark")]
        public virtual string Remark
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 菜单(页面)路径，包括页面文件名称
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public virtual string URL
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 权限承载类型，菜单、页面等
        /// </summary>
        [JsonProperty(PropertyName = "typecode")]
        public virtual int TypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 模块CODE,1204,1025,1026,1207,1208
        /// </summary>
        [JsonProperty(PropertyName = "modulecode")]
        public virtual int? ModuleCode
        {
            get; 
            set; 
        }        
		   
	}
}