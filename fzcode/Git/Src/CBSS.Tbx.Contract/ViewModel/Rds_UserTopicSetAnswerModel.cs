using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 口训答卷精简模型（不包括题目）
    /// </summary>
    public class Rds_UserTopicSetAnswerModel
    {
        /// <summary>
        /// 试卷id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 试卷名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>
        public long studentId { get; set; }

        public int? score { get; set; }

        /// <summary>
        /// 做题用时
        /// </summary>
        public string totalSpendTime { get; set; }
        /// <summary>
        /// 做题日期
        /// </summary>
        public string doDate { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int? stuScore { get; set; }
        /// <summary>
        /// 朗读
        /// </summary>
        public ImitationReading imitationReading { get; set; }
        /// <summary>
        /// 信息获取
        /// </summary>
        public InfoAcq infoAcq { get; set; }

        /// <summary>
        /// 信息转述及询问
        /// </summary>
        public InfoRepostAndQuery infoRepostAndQuery { get; set; }
    }
    public class InfoRepostAndQuery
    {
        public int id { get; set; }

        //public string title { get; set; }
        public int? stuScore { get; set; }

        public int? score { get; set; }

        public InfoRepost infoRepost { get; set; }

        public InfoQuery infoQuery { get; set; }
    }

    public class InfoRepost
    {
        public int repostId { get; set; }



        //public string answer { get; set; }

        public string stuAnswer { get; set; }
        //public string evaluateStd { get; set; }
        public int? stuScore { get; set; }
        public int? score { get; set; }
    }
    public class ImitationReading
    {
        /// <summary>
        /// 模仿朗读id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 学生答案
        /// </summary>
        public string stuAnswer { get; set; }
        /// <summary>
        /// 题目分数
        /// </summary>
        public int? score { get; set; }
        /// <summary>
        /// 学生得分
        /// </summary>
        public int? stuScore { get; set; }
    }

    public class InfoAcq
    {
        public int id { get; set; }
        /// <summary>
        /// 学生得分
        /// </summary>
        public int? stuScore { get; set; }

        public int? score { get; set; }

        public InfoAcqSection[] infoAcqSections { get; set; }
    }

    public class InfoAcqSection
    {
        public int sectionId { get; set; }

        public int? score { get; set; }
        /// <summary>
        /// 学生得分
        /// </summary>
        public int? stuScore { get; set; }

        public InfoAcqSectionMains[] infoAcqSectionMains { get; set; }
    }

    public class InfoAcqSectionMains
    {
        public int sectionMainId { get; set; }
        /// <summary>
        /// 学生得分
        /// </summary>
        public int? stuScore { get; set; }

        public int? score { get; set; }

        public InfoAcqItem[] infoAcqItems { get; set; }

    }
    public class InfoAcqItem
    {
        public int sectionItemId { get; set; }

        /// <summary>
        /// 学生得分
        /// </summary>
        public int? stuScore { get; set; }

        public int? score { get; set; }

        /// <summary>
        /// 学生答案
        /// </summary>
        public string stuAnswer { get; set; }

        //public string answer { get; set; }
    }
    public class InfoQueryItem
    {
        public int id { get; set; }
        /// <summary>
        /// 学生答案
        /// </summary>
        public string stuAnswer { get; set; }

        public int? score { get; set; }

        public int? stuScore { get; set; }

    }
    public class InfoQuery
    {
        public string id { get; set; }

        public int? score { get; set; }
        public int? stuScore { get; set; }

        public InfoQueryItem[] infoQueryItems { get; set; }
    }
}
