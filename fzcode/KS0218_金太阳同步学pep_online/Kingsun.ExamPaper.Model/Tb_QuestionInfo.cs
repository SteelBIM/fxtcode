using Kingsun.DB;
using System;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("Tb_QuestionInfo")]
    public partial class Tb_QuestionInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("QuestionID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string QuestionID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [FieldAttribute("QuestionTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionTitle { get; set; }

        /// <summary>
        /// 题干1
        /// </summary>
        [FieldAttribute("QuestionContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionContent { get; set; }

        /// <summary>
        /// 题干2
        /// </summary>
        [FieldAttribute("SecondContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondContent { get; set; }

        /// <summary>
        /// 音频地址
        /// </summary>
        [FieldAttribute("Mp3Url", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Mp3Url { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [FieldAttribute("ImgUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 所属大题ID（若为null，此题为大题）
        /// </summary>
        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ParentID { get; set; }

        /// <summary>
        /// 题型模板号
        /// </summary>
        [FieldAttribute("QuestionModel", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionModel { get; set; }

        /// <summary>
        /// 所属目录ID
        /// </summary>
        [FieldAttribute("CatalogID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogID { get; set; }

        /// <summary>
        /// 题目序号
        /// </summary>
        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 大题包含的小题数
        /// </summary>
        [FieldAttribute("MinQueCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? MinQueCount { get; set; }

        // <summary>
        /// 题目序号
        /// </summary>
        [FieldAttribute("Score", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public Decimal? Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Remark", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Remark { get; set; }

        //   [FieldAttribute("Score", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        //public decimal? Score { get; set; }

    }
}