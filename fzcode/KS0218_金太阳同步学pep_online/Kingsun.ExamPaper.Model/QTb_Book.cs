using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("QTb_Book")]
    public partial class QTb_Book : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookCover", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookCover { get; set; }

        /// <summary>
        /// 学科：1-语文，2-数学，3-英语
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        /// <summary>
        /// 年级ID，同步MOD
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 册别：1-上册，2-下册
        /// </summary>
        [FieldAttribute("BookReel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookReel { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsRemove", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRemove { get; set; }

    }
}