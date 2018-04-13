using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_UserAppoint")]
    /// <summary>
    /// 用户预约表
    /// </summary>
    public class Tb_UserAppoint : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [FieldAttribute("UserID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// CoursePeriodTimeID
        /// </summary>
        [FieldAttribute("CoursePeriodTimeID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int CoursePeriodTimeID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }
    }
}
