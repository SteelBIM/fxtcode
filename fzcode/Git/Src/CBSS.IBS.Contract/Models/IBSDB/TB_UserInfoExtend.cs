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
    [Table("TB_UserInfoExtend")]
    public partial class TB_UserInfoExtend 
    {
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
   
        public string UserID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
   
        public string EquipmentID { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>

        public string DeviceType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>

        public string IPAddress { get; set; }

    }
}
