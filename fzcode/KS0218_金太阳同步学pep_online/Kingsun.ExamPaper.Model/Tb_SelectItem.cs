using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("Tb_SelectItem")]
    public partial class Tb_SelectItem : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("QuestionID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string QuestionID { get; set; }

        /// <summary>
        /// 选择题文字选项
        /// </summary>
        [FieldAttribute("SelectItem", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SelectItem { get; set; }

        /// <summary>
        /// 选择题图片选项
        /// </summary>
        [FieldAttribute("ImgUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 选项顺序
        /// </summary>
        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        /// <summary>
        /// 是否为正确答案，1/0
        /// </summary>
        [FieldAttribute("IsAnswer", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsAnswer { get; set; }

    }
}
