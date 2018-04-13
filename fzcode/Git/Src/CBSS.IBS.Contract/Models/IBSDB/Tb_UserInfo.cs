using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using CBSS.Framework.Contract;

namespace CBSS.IBS.Contract
{
    [Auditable]
    [Table("Tb_UserInfo")]
    public partial class Tb_UserInfo 
    {
        
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否第一次登陆
        /// </summary>

        public string isLogState { get; set; }

        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>

        public int? IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
     
        public int? IsUser { get; set; }

        /// <summary>
        /// 用户注册的App版本
        /// </summary>

        public string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string UserImage { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public int? UserRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
      
        public string TrueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
  
        public string TelePhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
  
        public int? BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
    
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string UserName { get; set; }

    }
}
