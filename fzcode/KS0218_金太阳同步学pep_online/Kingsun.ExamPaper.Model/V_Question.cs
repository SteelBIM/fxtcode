using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("V_Question")]
    public partial class V_Question : Kingsun.DB.Action
    {
        [FieldAttribute("QuestionID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionID { get; set; }

        [FieldAttribute("QuestionTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionTitle { get; set; }

        [FieldAttribute("QuestionContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionContent { get; set; }

        [FieldAttribute("SecondContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondContent { get; set; }

        [FieldAttribute("Mp3Url", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Mp3Url { get; set; }

        [FieldAttribute("ImgUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImgUrl { get; set; }

        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ParentID { get; set; }

        [FieldAttribute("QuestionModel", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionModel { get; set; }

        [FieldAttribute("CatalogID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogID { get; set; }

        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        [FieldAttribute("MinQueCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? MinQueCount { get; set; }

        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        [FieldAttribute("CatalogName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CatalogName { get; set; }

        [FieldAttribute("BookName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookName { get; set; }

        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

        [FieldAttribute("Answer", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Answer { get; set; }

        [FieldAttribute("AnswerType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? AnswerType { get; set; }

        [FieldAttribute("Score", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? Score { get; set; }

    }
}