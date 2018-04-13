using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    /// <summary>
    /// 故事朗读资源表
    /// </summary>
    public class QTB_IDG_StoryRead
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 原文
        /// </summary>
        public string OriginalText { get; set; }
        /// <summary>
        /// 评测内容
        /// </summary>
        public string EvaluationContent { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }
        /// <summary>
        /// 所属年级
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 所属组别
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
