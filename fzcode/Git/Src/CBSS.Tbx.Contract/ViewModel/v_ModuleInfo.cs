using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{

    public class v_ModuleInfo
    {
        public string UserID { get; set; }
        public string BookID { get; set; }
        public string FirstTitleID { get; set; }
        public string SecondTitleID { get; set; }
        public string FirstModularID { get; set; }
        public string SecondModularID { get; set; }
        public int IsEnableOss { get; set; }
        public int Channel { get; set; }
        public IList<v_ListenSpeakInfo> Achievement { get; set; }
    }

    public class v_ListenSpeakInfo
    {
        public string AudioFileID { get; set; } //文件服务器ID
        public string Content { get; set; }
        public string Audio { get; set; }
        public string Image { get; set; }
        public string AdditionInfo { get; set; }
        public double Score { get; set; }
        public int? SerialNumber { get; set; }
        public int? TextSerialNumber { get; set; }
        public int? PlayTimes { get; set; }
    }


    public class UserVideoInfo
    {
        public string Type { get; set; }
        public string VideoType { get; set; }
        public int UserId { get; set; }
        public string VideoFileId { get; set; }
        public string BookId { get; set; }
        public int VideoNumber { get; set; }
        public string AppID { get; set; }
        public string VideoReleaseAddress { get; set; }
        public string VideoImageAddress { get; set; }
        public double TotalScore { get; set; }
        public string State { get; set; }
        public string VersionType { get; set; }
        public int IsEnableOss { get; set; }
        public Children[] children { get; set; }
        public string FirstTitleID { get; set; }
        public string SecondTitleID { get; set; }
        public string VideoTitle { get; set; }
        public string FirstModularID { get; set; }
    }

    public class Children
    {
        public int DialogueNumber { get; set; }
        public float DialogueScore { get; set; }
    }

    public class v_BookInfo
    {
        public int BookID { get; set; }
        public int FirstTitleID { get; set; }
        public string SecondTitleID { get; set; }
        public int FirstModularID { get; set; }
    }

    public class VideoRequset
    {
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string State { get; set; }
        public string PageIndex { get; set; }
        public int IsEnableOss { get; set; }
    }
    public class DeleteVedioInfo
    {
        public string IDStr { get; set; }
        public string State { get; set; }
    }
    public class UserSchoolRankParaModel
    {
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public string SchoolID { get; set; }
        public string ClassID { get; set; }
        public string BookID { get; set; }
        public string VideoNumber { get; set; }
        public string UserID { get; set; }
    }

    public class UserSchoolRankReponse
    {
        /// <summary>
        /// 当前用户排名
        /// </summary>
        public Redis_IntDubb_VideoInfoSort CurrentUserRank { get; set; }
        /// <summary>
        /// 用户排名列表
        /// </summary>
        public List<Redis_IntDubb_VideoInfoSort> RankList { get; set; }
    }

    public class UserVideoDetails
    {
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public string BookID { get; set; }
        public string VideoNumber { get; set; }
        public string AppID { get; set; }
        public string VideoID { get; set; }
        public string State { get; set; }
        public int Type { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class VideoRankingInfo
    {
        public int ID { get; set; }
        public int NumberOfOraise { get; set; }
        public double TotalScore { get; set; }
        public string VideoFileID { get; set; }
        public string UserImage { get; set; }
        public string VideoTitle { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsEnableOss { get; set; }
        public string TrueName { get; set; }
        public int UIsEnableOss { get; set; }
    }

    public class RankInfo
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime CreateTime { get; set; }
        public int NumberOfOraise { get; set; }
        public double TotalScore { get; set; }
        public string VideoTitle { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string NickName { get; set; }
        public string VideoFileId { get; set; }
    }

    public class VideoAchievement
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class UserVideoAchievement
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime CreateTime { get; set; }
        public int NumberOfOraise { get; set; }
        public double TotalScore { get; set; }
        public string UserName { get; set; }
        public string VideoTitle { get; set; }
        public string VideoType { get; set; }
        public string UserImage { get; set; }
        public string NickName { get; set; }
        public int State { get; set; }
        public int IsBool { get; set; }
        public string VideoFileId { get; set; }
        public UserVideoChildren[] children { get; set; }
    }

    public class UserVideoChildren
    {
        public int DialogueNumber { get; set; }
        public double DialogueScore { get; set; }
    }

    public class NumberOfOraiseState
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int State { get; set; }
    }
}
