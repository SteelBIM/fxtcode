using Kingsun.DB;
using System.Data;

namespace Kingsun.ExamPaper.Model
{
    [TableAttribute("QTb_Catalog")]
    public partial class QTb_Catalog : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CatalogID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? CatalogID { get; set; }

        /// <summary>
        /// 目录名
        /// </summary>
        [FieldAttribute("CatalogName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CatalogName { get; set; }

        /// <summary>
        /// 目录层级，1/2/3...
        /// </summary>
        [FieldAttribute("CatalogLevel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CatalogLevel { get; set; }

        /// <summary>
        /// 对应课本上的页码
        /// </summary>
        [FieldAttribute("PageNo", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PageNo { get; set; }

        /// <summary>
        /// 父CatalogID
        /// </summary>
        [FieldAttribute("ParentID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ParentID { get; set; }

        /// <summary>
        /// 所属课本ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 同一Book下的目录排序，不分层级
        /// </summary>
        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsRemove", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsRemove { get; set; }

        /// <summary>
        /// 音频
        /// </summary>
        [FieldAttribute("Mp3Url", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Mp3Url { get; set; }
    }
}