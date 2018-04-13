using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("QTb_Edition")]
    public partial class QTb_Edition : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ParentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsRemove", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRemove { get; set; }

        /// <summary>
        /// 父版本对应的MOD版本ID
        /// </summary>
        [FieldAttribute("MOD_ED", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? MOD_ED { get; set; }

    }
}