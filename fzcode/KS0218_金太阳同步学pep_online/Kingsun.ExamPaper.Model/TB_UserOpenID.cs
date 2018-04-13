using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("TB_UserOpenID")]
    public partial class TB_UserOpenID : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.CommonField | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 微信标识ID
        /// </summary>
        [FieldAttribute("OpenID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OpenID { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateDate { get; set; }

    }
}
