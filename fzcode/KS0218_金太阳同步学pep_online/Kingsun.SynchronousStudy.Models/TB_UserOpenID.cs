using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserOpenID")]
    public partial class TB_UserOpenID : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Telephone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Telephone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("OpenID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OpenID { get; set; }

    }
}
