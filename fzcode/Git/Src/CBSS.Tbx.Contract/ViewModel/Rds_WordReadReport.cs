using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 单词报告
    /// </summary>
    public class Rds_UserWordReadRecord
    {
        //public Guid? UserWordReadRecordID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 目录ID
        /// </summary>
        public int CatalogID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public decimal AvgScore { get; set; }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? DoDate { get; set; }
        /// <summary>
        /// 所有单词（读得差的单词，根据分数过滤）
        /// </summary>
        public List<Rds_UserWordReadRecordItem> Words { get; set; }

        ///// <summary>
        ///// 次数
        ///// </summary>
        //public int Frequency { get; set; }
        public Rds_UserWordReadRecord()
        {
            Words = new List<Rds_UserWordReadRecordItem>();
        }
    }

    /// <summary>
    /// 单个单词成绩
    /// </summary>
    public class Rds_UserWordReadRecordItem
    {
        public Guid? ID { get; set; }
        /// <summary>
        /// 原单词文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 学生完成的单词文本，包含颜色
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; }


        /// <summary>
        /// 录音
        /// </summary>
        public string AnswerSoundUrl { get; set; }

    }
}
