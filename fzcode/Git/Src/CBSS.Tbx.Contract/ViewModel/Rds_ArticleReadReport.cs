using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{

    /// <summary>
    /// 课文朗读
    /// </summary>
    public class Rds_UserArticleReadRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        //public Guid? UserArticleReadRecordID { get; set; }
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
        /// 课文标识
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 流利度
        /// </summary>
        public decimal Fluency { get; set; }
        /// <summary>
        /// 完成度
        /// </summary>
        public decimal Completeness { get; set; }
        /// <summary>
        /// 准确度
        /// </summary>
        public decimal CorrectRate { get; set; }

        /// <summary>
        /// 重读
        /// </summary>
   //     public decimal StressedReadRate { get; set; }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? DoDate { get; set; }

        /// <summary>
        /// 用户完成录音
        /// </summary>
        public string AnswerSoundUrl { get; set; }
        /// <summary>
        /// 所有句子（读得差的句子，根据分数过滤）
        /// </summary>
        public List<Rds_UserSentenceRecordItem> Sentences { get; set; }

        ///// <summary>
        ///// 次数
        ///// </summary>
        //public int Frequency { get; set; }

        public Rds_UserArticleReadRecord()
        {
            Sentences = new List<Rds_UserSentenceRecordItem>();
        }
    }
    /// <summary>
    /// 句子
    /// </summary>
    public class Rds_UserSentenceRecordItem
    {
        public Guid? ID { get; set; }
        /// <summary>
        /// 句子文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public string Score { get; set; }
        /// <summary>
        /// 学生朗读的文本,带颜色区分
        /// </summary>
        public string Answer { get; set; }
    }
}
