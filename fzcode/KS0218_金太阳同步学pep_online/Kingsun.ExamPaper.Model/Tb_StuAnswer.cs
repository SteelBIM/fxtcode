using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("Tb_StuAnswer")]
    public partial class Tb_StuAnswer : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StuAnswerID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string StuAnswerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StuCatID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string StuCatID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StuID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string StuID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CatalogID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("QuestionID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ParentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Answer", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Answer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsRight", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Score", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BestAnswer", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BestAnswer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BestIsRight", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BestIsRight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BestScore", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? BestScore { get; set; }

    }
}