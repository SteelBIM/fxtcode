using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("Tb_BlankAnswer")]
    public partial class Tb_BlankAnswer : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("QuestionID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionID { get; set; }

        /// <summary>
        /// 填空题答案
        /// </summary>
        [FieldAttribute("Answer", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Answer { get; set; }

        /// <summary>
        /// 答案类型（1：标准答案，>2:可选答案）
        /// </summary>
        [FieldAttribute("AnswerType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? AnswerType { get; set; }

    }
}
