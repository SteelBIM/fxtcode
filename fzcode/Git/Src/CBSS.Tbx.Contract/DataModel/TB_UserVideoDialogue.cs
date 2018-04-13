using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TB_UserVideoDialogue
    {
        /// <summary>
        /// 
        /// </summary>
        public string VideoType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户表主键ID
        /// </summary>
        public int UserVideoID { get; set; }

        /// <summary>
        /// 对白序号
        /// </summary>
        public int DialogueNumber { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        public double DialogueScore { get; set; }

        /// <summary>
        /// 文件服务器视频ID（用户配音的）
        /// </summary>
        public string VideoFileID { get; set; }
    }
}
