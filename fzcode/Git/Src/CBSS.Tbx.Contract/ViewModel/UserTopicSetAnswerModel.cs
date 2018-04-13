using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 口训答卷完整模型（包括题目）
    /// </summary>
    public class UserTopicSetAnswerModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public long studentId { get; set; }

        public int? stuScore { get; set; }

        public string totalSpendTime { get; set; }
        public int? score { get; set; }
        public string year { get; set; }

        public int type { get; set; }

        public string startAudio { get; set; }

        public int startDuration { get; set; }

        public string endAudio { get; set; }
        public int endDuration { get; set; }

        public string testAudio { get; set; }

        public int testDuration { get; set; }

        public string doDate { get; set; }
        public long createTime { get; set; }

        public ExImitationReading imitationReading { get; set; }

        public ExInfoAcq infoAcq { get; set; }

        public ExInfoRepostAndQuery infoRepostAndQuery { get; set; }
    }

    public class ExInfoRepostAndQuery
    {
        public int id { get; set; }

        public string title { get; set; }

        public int? stuScore { get; set; }

        public int? score { get; set; }

        public ExInfoRepost infoRepost { get; set; }

        public ExInfoQuery infoQuery { get; set; }
    }

    public class ExInfoRepost
    {
        public int repostId { get; set; }

        public string name { get; set; }
        public string content { get; set; }

        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string image { get; set; }

        public int imageReadTime { get; set; }
        public string subContent { get; set; }

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }
        public int recordingTime { get; set; }
        public string answer { get; set; }

        public string stuAnswer { get; set; }

        public string evaluateStd { get; set; }

        public int? stuScore { get; set; }
        public int? score { get; set; }
    }
    public class ExInfoQuery
    {
        public string id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
      
        public int? score { get; set; }
        public int? stuScore { get; set; }
        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public ExInfoQueryItem[] infoQueryItems { get; set; }
    }

    public class ExInfoQueryItem
    {
        public int id { get; set; }
        public string item { get; set; }
        public int readyTime { get; set; }
        public int recordingTime { get; set; }
        public string answer { get; set; }
        public string stuAnswer { get; set; }
        public string evaluateStd { get; set; }

        public int? stuScore { get; set; }
        public int? score { get; set; }

    }
    public class ExImitationReading
    {
        public int id { get; set; }
        public string title { get; set; }
      
        public string content { get; set; }

        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string subContent { get; set; }    

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }

        public int recordingTime { get; set; }

        public string answer { get; set; }

        public string stuAnswer { get; set; }

        public string evaluateStd { get; set; }

        public int? score { get; set; }

        public int? stuScore { get; set; }
    }

    public class ExInfoAcq
    {
        public int id { get; set; }

        public string title { get; set; }

        public int? stuScore { get; set; }
        public int? score { get; set; }
        public ExInfoAcqSection[] infoAcqSections { get; set; }
    }

    public class ExInfoAcqSection
    {
        public int sectionId { get; set; }
        public string sectionName { get; set; }
      
        public int? score { get; set; }
        public int? stuScore { get; set; }

        public ExInfoAcqSectionMains[] infoAcqSectionMains { get; set; }
    }

    public class ExInfoAcqSectionMains
    {
        public int sectionMainId { get; set; }
        public string content { get; set; }
        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string subContent { get; set; }

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }

        public int? stuScore { get; set; }
      
        public int? score { get; set; }

        public ExInfoAcqItem[] infoAcqItems { get; set; }

    }

    public class ExInfoAcqItem
    {
        public int sectionItemId { get; set; }

        public string item { get; set; }

        public string itemAudio { get; set; }

        public int itemDuration { get; set; }

        public int recordingTime { get; set; }

        public string answer { get; set; }

        public string stuAnswer { get; set; }

        public string evaluateStd { get; set; }

        public int? stuScore { get; set; }
        public int? score { get; set; }
    }
}
