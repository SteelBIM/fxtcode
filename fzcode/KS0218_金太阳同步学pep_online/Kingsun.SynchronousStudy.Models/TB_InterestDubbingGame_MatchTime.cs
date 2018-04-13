using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_InterestDubbingGame_MatchTime")]
    public partial class TB_InterestDubbingGame_MatchTime : Kingsun.DB.Action
    {
        /// <summary>
        /// 报名开始时间
        /// </summary>
        [FieldAttribute("SignUpStartTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? SignUpStartTime { get; set; }
        /// <summary>
        /// 报名结束时间
        /// </summary>
        [FieldAttribute("SignUpEndTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? SignUpEndTime { get; set; }
        /// <summary>
        /// 初赛开始时间
        /// </summary>
        [FieldAttribute("FirstGameStartTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? FirstGameStartTime { get; set; }
        /// <summary>
        /// 初赛结束时间
        /// </summary>
        [FieldAttribute("FirstGameEndTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? FirstGameEndTime { get; set; }
        /// <summary>
        /// 复赛开始时间
        /// </summary>
        [FieldAttribute("SecondGameStartTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? SecondGameStartTime { get; set; }
        /// <summary>
        /// 复赛结束时间
        /// </summary>
        [FieldAttribute("SecondGameEndTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? SecondGameEndTime { get; set; }
        /// <summary>
        /// 决赛开始时间
        /// </summary>
        [FieldAttribute("FinalsStartTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? FinalsStartTime { get; set; }
        /// <summary>
        /// 决赛结束时间
        /// </summary>
        [FieldAttribute("FinalsEndTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? FinalsEndTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 版本编号
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        [FieldAttribute("VersionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionName { get; set; }
    }
}
