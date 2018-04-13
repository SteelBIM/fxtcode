using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("V_Resource")]
    public partial class V_Resource : Kingsun.DB.Action
    {
        [FieldAttribute("ResID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ResID { get; set; }

        [FieldAttribute("ResUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResUrl { get; set; }

        [FieldAttribute("ResMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResMD5 { get; set; }

        [FieldAttribute("ResVersion", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResVersion { get; set; }

        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        [FieldAttribute("ResUpTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ResUpTimes { get; set; }

        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

    }
}
