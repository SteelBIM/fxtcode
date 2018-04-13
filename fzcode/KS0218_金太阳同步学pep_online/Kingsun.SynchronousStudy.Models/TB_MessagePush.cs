using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_MessagePush")]
    public partial class TB_MessagePush : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 打开次数
        /// </summary>
        [FieldAttribute("OpenNumber", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? OpenNumber { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        [FieldAttribute("TestDsc", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TestDsc { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [FieldAttribute("StartTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [FieldAttribute("EndTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 按钮图片
        /// </summary>
        [FieldAttribute("ButtonImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ButtonImage { get; set; }

        /// <summary>
        /// 推送版本
        /// </summary>
        [FieldAttribute("PushEdition", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PushEdition { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        [FieldAttribute("JumpLink", null, EnumFieldUsage.CommonField, DbType.String)]
        public string JumpLink { get; set; }

        /// <summary>
        /// 使用时长
        /// </summary>
        [FieldAttribute("UseTime", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UseTime { get; set; }

        /// <summary>
        /// 使用天数
        /// </summary>
        [FieldAttribute("Number", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Number { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [FieldAttribute("ClassID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ClassID { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [FieldAttribute("MessageTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string MessageTitle { get; set; }

        /// <summary>
        /// 标题状态
        /// </summary>
        [FieldAttribute("TitleState", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TitleState { get; set; }

        /// <summary>
        /// 推送图片
        /// </summary>
        [FieldAttribute("Image", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Image { get; set; }

    }
}
