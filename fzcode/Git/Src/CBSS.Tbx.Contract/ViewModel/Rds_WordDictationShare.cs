using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class Rds_UserWordDictationShare
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
        public decimal Score { get; set; }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? DoDate { get; set; }
        /// <summary>
        /// 所有单词（读得差的单词，根据分数过滤）
        /// </summary>
        public List<Rds_UserWordDictationRecordItem> Words { get; set; }

        /// <summary>
        /// 单词总数
        /// </summary>
        public int WordsCount { get; set; }
        /// <summary>
        /// 正确数
        /// </summary>
        public int RightCount { get; set; }

        public Rds_UserWordDictationShare()
        {
            Words = new List<Rds_UserWordDictationRecordItem>();
        }
    }
}
