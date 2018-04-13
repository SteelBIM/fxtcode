using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 单词报告分享
    /// </summary>
    public class Rds_WordReadShare
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
        /// 完成日期
        /// </summary>
        public DateTime? DoDate { get; set; }
        /// <summary>
        /// 所有单词（读得差的单词，根据分数过滤）
        /// </summary>
        public List<Rds_UserWordReadRecordItem> Words { get; set; }

        public Rds_WordReadShare()
        {
            Words = new List<Rds_UserWordReadRecordItem>();
        }
    }
   
}
