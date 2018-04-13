using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    /// <summary>
    /// 用户总成绩表
    /// </summary>
    [TableAttribute("TB_InterestDubbingGame_UserContentsRecord")]
    public class TB_InterestDubbingGame_UserContentsRecord : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
      
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// 配音标题
        /// </summary>
        [FieldAttribute("DubbingTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string DubbingTitle { get; set; }
        /// <summary>
        /// 配音文件路径
        /// </summary>
        [FieldAttribute("DubbingFilePath", null, EnumFieldUsage.CommonField, DbType.String)]
        public string DubbingFilePath { get; set; }
        /// <summary>
        /// 配音分数
        /// </summary>
        [FieldAttribute("DubbingScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double DubbingScore { get; set; }
        /// <summary>
        /// 配音类别（课本剧配音0/故事朗读1）
        /// </summary>
        [FieldAttribute("Type", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
       [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 资源视频ID
        /// </summary>
         [FieldAttribute("VideoID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VideoID { get; set; }
    }
}
