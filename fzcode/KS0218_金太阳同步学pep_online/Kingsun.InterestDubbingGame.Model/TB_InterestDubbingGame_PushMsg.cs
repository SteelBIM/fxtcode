using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    [TableAttribute("TB_InterestDubbingGame_PushMsg")]
    public class TB_InterestDubbingGame_PushMsg : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// VersionID
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }
        /// <summary>
        /// VersionNumber
        /// </summary>
        [FieldAttribute("VersionNumber", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionNumber { get; set; }
        /// <summary>
        /// VersionName
        /// </summary>
        [FieldAttribute("VersionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionName { get; set; }
        /// <summary>
        /// IdentityType
        /// </summary>
        [FieldAttribute("IdentityType", null, EnumFieldUsage.CommonField, DbType.String)]
        public string IdentityType { get; set; }
        /// <summary>
        /// Jump
        /// </summary>
        [FieldAttribute("Jump", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Jump { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        [FieldAttribute("Content", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Content { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int State { get; set; }
        /// <summary>
        /// DelState
        /// </summary>
        [FieldAttribute("DelState", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int DelState { get; set; }
        /// <summary>
        /// PushTime
        /// </summary>
        [FieldAttribute("PushTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? PushTime { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }
    }
}
