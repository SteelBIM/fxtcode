using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 模块管理
    /// </summary>
    public class ModuleManage
    {
        public int MarketBookID { get; set; }
        /// <summary>
        /// 书籍名称
        /// </summary>
        public string MarketBookName { get; set; }
        /// <summary>
        /// 书籍类别
        /// </summary>
        public string MarketBookClass { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string ModuleStr { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int MarketClassifyID { get; set; }
        /// <summary>
        /// 书籍排序
        /// </summary>
        public int BookSort { get; set; }
        /// <summary>
        /// 分类排序ID
        /// </summary>
        public int ClassifySort { get; set; }
    }
}
