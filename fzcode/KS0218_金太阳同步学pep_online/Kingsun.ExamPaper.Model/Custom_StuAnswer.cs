namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义上传的答案
    /// </summary>
    public class Custom_StuAnswer
    {
        public string QuestionID { get; set; }
        public string ParentID { get; set; }
        public string Answer { get; set; }
        public decimal Score { get; set; }
        public int IsRight { get; set; }
        public string CatalogID { get; set; }
    }
}
