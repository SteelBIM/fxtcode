using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.ResourcesManager.Contract.DataModel
{
    /// <summary>
    /// 视频对白表
    /// </summary>
    public class InterestDubbingFragment
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
      
        /// <summary>
        /// 对白
        /// </summary>
        public string DialogueText { get; set; }

        /// <summary>
        /// 对白顺序号
        /// </summary>
        public int DialogueNumber { get; set; }

        /// <summary>
        /// 对白开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 对白结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        public int ResourceID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int MODBookID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int OldMODBookID { get; set; }
    }
}
