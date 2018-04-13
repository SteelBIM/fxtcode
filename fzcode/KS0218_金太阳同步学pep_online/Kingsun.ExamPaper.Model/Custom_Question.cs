using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义题目信息
    /// </summary>
    public class Custom_Question
    {
        public string QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionModel { get; set; }
        public string QuestionContent { get; set; }
        public string SecondContent { get; set; }
        public string Mp3Url { get; set; }
        public string ImgUrl { get; set; }
        public string ParentID { get; set; }
        public int CatalogID { get; set; }
        public int Sort { get; set; }
        public int MinQueCount { get; set; }
        public float Score { get; set; }
        public IList<Tb_SelectItem> SelectList { get; set; }
        public IList<Tb_BlankAnswer> BlankAnswer { get; set; }
        public Tb_StuAnswer StuAnswer { get; set; }
    }
}
