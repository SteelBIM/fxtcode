using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudyHopeChina.Web
{
    [TableAttribute("tb_Score")]
    public partial class tb_Score : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Userid", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Userid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Articleid", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Articleid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Score", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Filepath", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Filepath { get; set; }

    }
}
