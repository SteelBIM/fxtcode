using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("tb_File")]
    public partial class TbFile : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Filename", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Filename { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Filepath", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Filepath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Filesize", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? Filesize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Filetype", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Filetype { get; set; }

    }
}
