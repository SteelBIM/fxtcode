using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_VoteRecord")]
    public partial class TB_VoteRecord : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserIP", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

    }
}
