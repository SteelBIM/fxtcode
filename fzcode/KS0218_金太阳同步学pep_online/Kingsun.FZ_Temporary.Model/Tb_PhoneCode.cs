using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.FZ_Temporary.Model
{
    [TableAttribute("Tb_PhoneCode")]
    public partial class Tb_PhoneCode : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Code", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EndDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("State", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

    }
}
