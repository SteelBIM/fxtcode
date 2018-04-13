using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.ResourcesManager.Contract.DataModel
{
    /// <summary>
    /// 视频详情表
    /// </summary>
    public class InterestDubbingResource
    {
        /// <summary>
        /// 视频难度程度
        /// </summary>
        public int VideoDifficulty { get; set; }

        /// <summary>
        /// 视频状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 完整视频
        /// </summary>
        public string CompleteVideo { get; set; }

        /// <summary>
        /// 背景音频
        /// </summary>
        public string BackgroundAudio { get; set; }

        /// <summary>
        /// 视频封面
        /// </summary>
        public string VideoCover { get; set; }

        /// <summary>
        /// 视频简介
        /// </summary>
        public string VideoDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VideoTime { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        /// 视频序号
        /// </summary>
        public int VideoNumber { get; set; }
        
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 静音视频
        /// </summary>
        public string MuteVideo { get; set; }

        /// <summary>
        /// mod目录id
        /// </summary>
        public int MODBookCatalogID { get; set; }

        /// <summary>
        /// 老mod目录id
        /// </summary>
        public int OldMODBookCatalogID { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        public int ModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        public int SecondModularID { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ResourceID { get; set; }

        /// <summary>
        /// mod书籍ID
        /// </summary>
        public int MODBookID { get; set; }

        /// <summary>
        /// 老mod书籍ID
        /// </summary>
        public int OldMODBookID { get; set; }
        
    }
}
