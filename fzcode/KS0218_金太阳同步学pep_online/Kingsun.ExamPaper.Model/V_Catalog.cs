using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("V_Catalog")]
    public partial class V_Catalog : Kingsun.DB.Action
    {
        [FieldAttribute("CatalogID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogID { get; set; }

        [FieldAttribute("CatalogName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CatalogName { get; set; }

        [FieldAttribute("PageNo", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PageNo { get; set; }

        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        [FieldAttribute("IsRemove", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRemove { get; set; }

        [FieldAttribute("CatalogLevel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogLevel { get; set; }

        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ParentID { get; set; }

        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        [FieldAttribute("MOD_ED", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? MOD_ED { get; set; }

        [FieldAttribute("BookName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookName { get; set; }

        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

    }
}
