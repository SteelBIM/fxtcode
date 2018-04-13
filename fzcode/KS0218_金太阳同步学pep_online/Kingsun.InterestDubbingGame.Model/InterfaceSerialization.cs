using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    public class InterfaceSerialization
    {

    }

    public class IDGBookDramat
    {
        public string UserId { get; set; }
    }

    public class IDGUserContentsRecord
    {
        public int UserId { get; set; }
        public string DubbingTitle { get; set; }
        public string DubbingFilePath { get; set; }
        public double DubbingScore { get; set; }
        public int Type { get; set; }
        public int VideoID { get; set; }
    }


    public class IDGGameScore
    {
        public string ClassID { get; set; }
        public int pageNumber { get; set; }
    }

    public class IDGUserID
    {
        public string UserID { get; set; }
    }

    public class IDGUserInfo
    {
        public int UserID { get; set; }
        public string UserImage { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateTime { get; set; }

    }

    public class IDGUserScore
    {
        public int UserID { get; set; }
        public double TotalScore { get; set; }
        public double BookPlayScore { get; set; }
        public double StoryReadScore { get; set; }
        public int VoteNum { get; set; }
    }
    public class IDGGameScoreInfoList
    {
        public int UserID { get; set; }
        public double TotalScore { get; set; }
        public string UserImg { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsStudy { get; set; }
    }

    public class UserStoryInfo
    {
        public int UserID { get; set; }
        public string DubbingTitle { get; set; }
        public string DubbingFilePath { get; set; }
        public double DubbingScore { get; set; }
        public DateTime CreateDate { get; set; }
        public int VideoID { get; set; }
        public string EvaluationContent { get; set; }
        public string OriginalText { get; set; }
    }
}
