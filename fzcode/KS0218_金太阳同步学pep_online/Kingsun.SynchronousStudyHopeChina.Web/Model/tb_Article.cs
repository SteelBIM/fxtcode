using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudyHopeChina.Web.Model
{
    [Kingsun.DB.TableAttribute("tb_Article")]
    public partial class tb_Article : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ATitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ATitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("APeriod", null, EnumFieldUsage.CommonField, DbType.String)]
        public string APeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ARemark", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ARemark { get; set; }

    }
}
