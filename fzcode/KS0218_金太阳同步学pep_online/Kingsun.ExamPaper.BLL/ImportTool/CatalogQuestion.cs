using System.Collections.Generic;

namespace Kingsun.ExamPaper.BLL.ImportTool
{
    /// <summary>
    /// Excel对应的目录模板
    /// </summary>
    public class CatalogQuestion
    {
        /// <summary>
        /// 数据库对应的目录ID
        /// </summary>
        public int CatalogID { get; set; }
        /// <summary>
        /// 目录编号
        /// </summary>
        public int CatalogNo { get; set; }
        /// <summary>
        /// 父级目录编号
        /// </summary>
        public int ParentCatalogNo { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 对应页码
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 目录对应MP3
        /// </summary>
        public string Mp3Url { get; set; }
        /// <summary>
        /// 目录包含的题目列表
        /// </summary>
        public IList<Question> QuestionList { get; set; }
    }
    /// <summary>
    /// Excel对应的题目模板
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 大题号
        /// </summary>
        public int ParentNo { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string QuestionModel { get; set; }
        /// <summary>
        /// 目录编号
        /// </summary>
        public int CatalogNo { get; set; }
        /// <summary>
        /// 小题号
        /// </summary>
        public int QuestionNo { get; set; }
        /// <summary>
        /// 标题文字
        /// </summary>
        public string QuestionTitle { get; set; }
        /// <summary>
        /// 标题内容
        /// </summary>
        public string QuestionContent { get; set; }
        /// <summary>
        /// 副标题内容
        /// </summary>
        public string SecondContent { get; set; }
        /// <summary>
        /// 音频
        /// </summary>
        public string Mp3Url { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string BlankAnswer { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public IList<SelItem> SelectList { get; set; }

        public decimal? Score { get; set; }
    }
    /// <summary>
    /// Excel对应的选项
    /// </summary>
    public class SelItem
    {
        public string SelectItem { get; set; }
        public int IsAnswer { get; set; }
        public string ImgUrl { get; set; }
    }
}
