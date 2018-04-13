using System;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义成绩报告结构
    /// </summary>
    public class Custom_StuCatalogList
    {
        public IList<StuCatalog> StuCatalogList { get; set; }
    }


    /// <summary>
    /// 自定义成绩报告结构
    /// </summary>
    public class StuCatalog
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        public int ParentID { get; set; }
        public string ParentCatalogName { get; set; }
        public float TotalScore { get; set; }
        public float BestTotalScore { get; set; }
        public DateTime DoDate { get; set; }
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public IList<PQ> QuestionList { get; set; }
        
    }
    /// <summary>
    /// 自定义成绩报告大题结构
    /// </summary>
    public class PQ
    {
        public string QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public int Sort { get; set; }
        public float BestScore { get; set; }
        public string BestAnswer { get; set; }
        public int BestIsRight { get; set; }
        public string ParentID { get; set; }
        public string QuestionModel { get; set; }
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
    }
}
