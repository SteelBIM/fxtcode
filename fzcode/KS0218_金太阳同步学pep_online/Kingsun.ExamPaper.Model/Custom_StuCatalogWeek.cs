using System;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义成绩报告结构
    /// </summary>
    public class Custom_StuCatalogListWeek
    {
        public IList<StuCatalogWeek> StuCatalogWeek { get; set; }
    }
    /// <summary>
    /// 自定义成绩报告大题结构
    /// </summary>
    public class StuCatalogWeek
    {
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public int BestScore { get; set; }
        public int LastBestScore { get; set; }
    }
}
