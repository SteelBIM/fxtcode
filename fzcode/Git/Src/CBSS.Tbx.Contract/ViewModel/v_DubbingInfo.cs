using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 趣配音视频信息
    /// </summary>
    public class V_DubbingInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 书籍ID
        /// </summary>
        public long BookID { get; set; }
        /// <summary>
        /// 书籍名称
        /// </summary>
        public string  BookName { get; set; }
        /// <summary>
        /// 一级目录ID
        /// </summary>
        public int FirstTitleID { get; set; }
        /// <summary>
        /// 一级目录
        /// </summary>
        public string FirstTitle { get; set; }
        /// <summary>
        /// 二级目录ID
        /// </summary>
        public int SecondTitleID { get; set; }
        /// <summary>
        /// 二级目录
        /// </summary>
        public string SecondTitle { get; set; }
        /// <summary>
        /// 一级模块ID
        /// </summary>
        public int FirstModularID { get; set; }
        /// <summary>
        /// 一级模块
        /// </summary>
        public string FirstModular { get; set; }
        /// <summary>
        /// 二级模块ID
        /// </summary>
        public int SecondModularID { get; set; }
        /// <summary>
        /// 二级模块名称(1-课内配音 2-电影配音)
        /// </summary>
        public string SecondModular { get; set; }
        /// <summary>
        /// 视频顺序
        /// </summary>
        public int VideoNumber { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoTitle { get; set; }
        /// <summary>
        /// 静音视频
        /// </summary>
        public string MuteVideo { get; set; }
        /// <summary>
        /// 完整视频
        /// </summary>
        public string CompleteVideo { get; set; }
        /// <summary>
        /// 背景视频
        /// </summary>
        public string BackgroundAudio { get; set; }
        /// <summary>
        /// 视频封面
        /// </summary>
        public string VideoCover { get; set; }
        /// <summary>
        /// 视频难度
        /// </summary>
        public int VideoDifficulty { get; set; }
        /// <summary>
        /// 视频描述
        /// </summary>
        public string VideoDesc { get; set; }

        public List<DubbingFragments> dubbingFragments { get; set; }

    }

    /// <summary>
    /// 趣配音视频子句信息
    /// </summary>
    public class DubbingFragments
    {
        /// <summary>
        /// 子视频ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 子视频顺序
        /// </summary>
        public int DialogueNumber { get; set; }
        /// <summary>
        /// 基于整个视频的开始时间(毫秒)
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// 基于整个视频的结束时间（毫秒）
        /// </summary>
        public int EndTime { get; set; }
        /// <summary>
        /// 内容配文
        /// </summary>
        public string DialogueText { get; set; }
    }

    public class VideoInfo
    {
        public int ID { get; set; }
        public int VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoImageAddress { get; set; }
        public string VideoReleaseAddress { get; set; }
        public double TotalScore { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string State { get; set; }
        public string VideoType { get; set; }
    }
}
