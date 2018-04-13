using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    /// <summary>
    /// 课本剧
    /// </summary>
    public class QTB_IDG_BookDrama
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 视频标题
        /// </summary>
        public string VideoTitle { get; set; }
        /// <summary>
        /// 视频序号
        /// </summary>
        public int VideoNumber { get; set; }
        /// <summary>
        /// 静音视频
        /// </summary>
        public string MuteVideo { get; set; }
        /// <summary>
        /// 完整视频
        /// </summary>
        public string CompleteVideo { get; set; }
        /// <summary>
        /// 背景音频
        /// </summary>
        public string BackgroundAudio { get; set; }
        /// <summary>
        /// 视频封图
        /// </summary>
        public string VideoCover { get; set; }
        /// <summary>
        /// 视频简介
        /// </summary>
        public string VideoDesc { get; set; }
        /// <summary>
        /// 视频难易程度
        /// </summary>
        public int VideoDifficulty { get; set; }
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
