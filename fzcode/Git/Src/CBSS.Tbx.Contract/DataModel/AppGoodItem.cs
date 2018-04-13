using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("AppGoodItem")]
    public class AppGoodItem
    {
        /// <summary>
        /// 应用选择商品策略ID
        /// </summary>

        public int AppGoodItemID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 商品策略ID
        /// </summary>

        public int GoodID { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }
    }
}
