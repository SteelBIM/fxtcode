using CBSS.Framework.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 市场书籍
    /// </summary>
 
    public class V_MarketBook
    {
        /// <summary>
        /// 市场书籍ID
        /// </summary>
        public int MarketBookID { get; set; }
        /// <summary>
        /// 市场分类ID
        /// </summary>
        public int MarketClassifyId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string MarketClassifyName { get; set; }
        /// <summary>
        /// 自定义市场书籍名称，使用MOD名称，值为空
        /// </summary>
        public string MarketBookName { get; set; }
        /// <summary>
        /// 自定义封面图，如果使用MOD封面，值为空
        /// </summary>
        public string MarketBookCover { get; set; }
        /// <summary>
        /// MOD课本ID，不使用MOD课本ID,值为0
        /// </summary>
        public string MODBookName { get; set; }
        public string MODBookCover { get; set; }
        public int MODBookID { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }

        public List<V_MarketBookCatalog> Catalogs { get; set; }
    }
}
