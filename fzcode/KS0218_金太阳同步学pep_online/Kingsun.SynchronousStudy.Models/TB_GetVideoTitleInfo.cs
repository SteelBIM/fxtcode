using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_GetVideoTitleInfo")]
    public partial class TB_GetVideoTitleInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsEnableOss", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VideoNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int FirstTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SecondTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoImageAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoImageAddress { get; set; }

    }
}
