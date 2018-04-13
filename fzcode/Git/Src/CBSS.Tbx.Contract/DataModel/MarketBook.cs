using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    /// <summary>
    /// 市场书籍
    /// </summary>
    [Auditable]
    [Table("MarketBook")]
    public class MarketBook
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
        /// 自定义市场书籍名称，使用MOD名称，值为空
        /// </summary>
        public string MarketBookName { get; set; }

        /// <summary>
        /// 学段
        /// </summary>
        public int Stage { get; set; }
        /// <summary>
        /// 自定义封面图，如果使用MOD封面，值为空
        /// </summary>
        public string MarketBookCover { get; set; }
        /// <summary>
        /// MOD课本ID，不使用MOD课本ID,值为0
        /// </summary>

        public int MODBookID { get; set; }
        public string MODBookName { get; set; }
        public string MODBookCover { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }

        /// <summary>
        /// 国际标准图书编号
        /// </summary>
        public string ISBN { get; set; }

       
    }
}
