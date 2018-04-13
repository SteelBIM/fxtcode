using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserActiveVideo")]
    public partial class TB_UserActiveVideo : Kingsun.DB.Action
    {
        /// <summary>
        /// 奖项1（萌萌哒）
        /// </summary>
        [FieldAttribute("PrizeOne", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PrizeOne { get; set; }

        /// <summary>
        /// 奖项2（淘气包）
        /// </summary>
        [FieldAttribute("PrizeTwo", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PrizeTwo { get; set; }

        /// <summary>
        /// 奖项3（机灵鬼）
        /// </summary>
        [FieldAttribute("PrizeThree", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PrizeThree { get; set; }

        /// <summary>
        /// 奖项4（学霸君）
        /// </summary>
        [FieldAttribute("PrizeFour", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PrizeFour { get; set; }

        /// <summary>
        /// 奖项5（小大人）
        /// </summary>
        [FieldAttribute("PrizeFive", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PrizeFive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户视频ID
        /// </summary>
        [FieldAttribute("UserVideoID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserVideoID { get; set; }

    }
}
