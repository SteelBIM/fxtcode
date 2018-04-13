using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("V_Book")]
    public partial class V_Book : Kingsun.DB.Action
    {
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        [FieldAttribute("BookName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookName { get; set; }

        [FieldAttribute("BookCover", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookCover { get; set; }

        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        [FieldAttribute("IsRemove", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRemove { get; set; }

        [FieldAttribute("MOD_ED", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? MOD_ED { get; set; }

        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

    }
}
