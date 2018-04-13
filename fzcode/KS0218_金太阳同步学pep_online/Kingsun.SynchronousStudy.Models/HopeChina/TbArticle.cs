using System.Data;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [Kingsun.DB.TableAttribute("tb_Article")]
    public partial class TbArticle : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ATitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ATitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AContent", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("APeriod", null, EnumFieldUsage.CommonField, DbType.String)]
        public string APeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ARemark", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ARemark { get; set; }

    }
}
