using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("QTb_Resource")]
    public partial class QTb_Resource : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ResID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ResID { get; set; }

        /// <summary>
        /// 资源包路径
        /// </summary>
        [FieldAttribute("ResUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResUrl { get; set; }

        /// <summary>
        /// 资源包MD5值
        /// </summary>
        [FieldAttribute("ResMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResMD5 { get; set; }

        /// <summary>
        /// 资源包版本，拼凑规则：EditionID.(GradeID-1).BookReel.ResUpTimes
        /// </summary>
        [FieldAttribute("ResVersion", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        /// <summary>
        /// 资源包更新次数
        /// </summary>
        [FieldAttribute("ResUpTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ResUpTimes { get; set; }

    }
}
