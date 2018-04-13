using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    /// <summary>
    /// 课本剧对白表
    /// </summary>
    public class QTB_IDG_BookDialogue
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 视频序号
        /// </summary>
        public int VideoNumber { get; set; }
        /// <summary>
        /// 分视频序号
        /// </summary>
        public int DialogueNumber { get; set; }
        /// <summary>
        /// 分视频文本
        /// </summary>
        public string DialogueText { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
