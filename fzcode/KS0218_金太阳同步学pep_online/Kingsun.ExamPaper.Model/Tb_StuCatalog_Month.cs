using Kingsun.DB;
using System;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("Tb_StuCatalog_Month")]
    public partial class Tb_StuCatalog_Month : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StuCatID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string StuCatID { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        [FieldAttribute("StuID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string StuID { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [FieldAttribute("CatalogID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogID { get; set; }

        /// <summary>
        /// 最近一次成绩
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? TotalScore { get; set; }

        /// <summary>
        /// 历史最佳成绩
        /// </summary>
        [FieldAttribute("BestTotalScore", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? BestTotalScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("DoDate", null, EnumFieldUsage.CommonField, DbType.String)]
        public DateTime DoDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AnswerNum", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? AnswerNum { get; set; }

    }
}