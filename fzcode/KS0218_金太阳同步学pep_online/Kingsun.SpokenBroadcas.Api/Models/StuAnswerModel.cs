using Kingsun.ExamPaper.Model;
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
        public float? TotalScore { get; set; }
        public IList<Custom_StuAnswer> AnswerList { get; set; }
    }
}