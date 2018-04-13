using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.Api.Models
{
    /// <summary>
    /// 批量提交答案原型
    /// </summary>
    public class SubmitModel
    {
        public string UserID { get; set; }
        public int? CatalogID { get; set; }
        public decimal? TotalScore { get; set; }

        public DateTime? DoDate { get; set; }//hlw加
        public IList<Custom_StuAnswer> AnswerList { get; set; }
    }

}