using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义一级目录
    /// </summary>
    public class Custom_Catalog
    {
        /// <summary>
        /// 目录ID
        /// </summary>
        public int CatalogID { get; set; }
        /// <summary>
        /// 目录名
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 做题人数
        /// </summary>
        public int AnswerNum { get; set; }
        /// <summary>
        /// 前段显示做题人数
        /// </summary>
        public float AnswerNumShow { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 最近是否已提交报告：0-进入做题页，1-进入报告页
        /// </summary>
        public int IsSubmit { get; set; }

        public string Mp3Url { get; set; }
        /// <summary>
        /// 子目录
        /// </summary>
        public IList<Custom_SecondCatalog> SecondCatalogList { get; set; }
    }
    /// <summary>
    /// 自定义二级目录
    /// </summary>
    public class Custom_SecondCatalog
    {
        /// <summary>
        /// 目录ID
        /// </summary>
        public int CatalogID { get; set; }
        /// <summary>
        /// 目录名
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 做题人数
        /// </summary>
        public int AnswerNum { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 最近是否已提交报告：0-进入做题页，1-进入报告页
        /// </summary>
        public int IsSubmit { get; set; }

        public string Mp3Url { get; set; }
    }
}
