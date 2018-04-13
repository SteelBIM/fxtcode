using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
   public class v_AppBookCatalogModuleItem: Module
    {
        public int AppBookCatalogModuleItemID { get; set; }
        /// <summary>
        /// 购买前图片
        /// </summary>
        public string BeforeBuyingImg { get; set; }
        /// <summary>
        /// 购买后图片
        /// </summary>
        public string BuyLaterImg { get; set; }
        /// <summary>
        /// 模块前端显示名称
        /// </summary>
        public string ModuleNameShow { get; set; }
        /// <summary>
        /// 勾选状态0:否，1:是
        /// </summary>
        public int StatusShow { get; set; }
        public int MarketBookCatalogID { get; set; }
    }
}
