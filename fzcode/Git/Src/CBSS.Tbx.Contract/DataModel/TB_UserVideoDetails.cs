using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    /// <summary>
    /// 趣配音用户数据
    /// </summary>
    public class TB_UserVideoDetails
    {
        /// <summary>
        /// 获赞数量
        /// </summary>
        public int NumberOfOraise { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayTimes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int VersionType { get; set; }

        /// <summary>
        /// 是否是活动视频（0：否，1：是）
        /// </summary>
        public string VideoType { get; set; }

        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 文件服务器视频ID
        /// </summary>
        public string VideoFileID { get; set; }

        /// <summary>
        /// 视频发布地址
        /// </summary>
        public string VideoReleaseAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VideoImageAddress { get; set; }

        /// <summary>
        /// 配音总成绩
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int column1 { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        public int VersionID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// 视频序号
        /// </summary>
        public int VideoNumber { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        public int VideoID { get; set; }
    }
}
