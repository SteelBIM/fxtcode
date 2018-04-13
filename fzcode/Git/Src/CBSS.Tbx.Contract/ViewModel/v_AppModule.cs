using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_AppModule : MarketBook
    {
        public int AppMarketBookID { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
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
