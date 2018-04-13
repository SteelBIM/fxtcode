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
    public class Rds_ArticleReadShare
    {
        /// <summary>
        /// 分享ID
        /// </summary>
        public Guid? ShareID { get; set; }
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
    //    public decimal StressedReadRate { get; set; }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? DoDate { get; set; }

        /// <summary>
        /// 录音
        /// </summary>
        public string AnswerSoundUrl { get; set; }

        /// <summary>
        /// 所有句子（读得差的句子，根据分数过滤）
        /// </summary>
        public List<Rds_UserSentenceRecordItem> Sentences { get; set; }

        public Rds_ArticleReadShare()
        {
            Sentences = new List<Rds_UserSentenceRecordItem>();
        }
    }
}
