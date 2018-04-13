using System;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义成绩报告结构
    /// </summary>
    public class Custom_StuCatalog
    {
        public decimal TotalScore { get; set; }
        public decimal BestTotalScore { get; set; }
        public DateTime DoDate { get; set; }
        public IList<Custom_PQ> QuestionList { get; set; }
    }
    /// <summary>
    /// 自定义成绩报告大题结构
    /// </summary>
    public class Custom_PQ
    {
        public string QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public int Sort { get; set; }

        public decimal? Score { get; set; }
        public decimal BestScore { get; set; }
        public string BestAnswer { get; set; }
        public int BestIsRight { get; set; }
        public string ParentID { get; set; }
        public string QuestionModel { get; set; }
    }

    /// <summary>
    /// 批量提交答案原型
    /// </summary>
    public class CSubmitModel
    {
        public string UserID { get; set; }
        public int? CatalogID { get; set; }
        public decimal? TotalScore { get; set; }
        public DateTime? DoDate { get; set; }//hlw加
        public IList<Custom_StuAnswer> AnswerList { get; set; }
    }
}
