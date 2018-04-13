using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class V_AppBookCatalogModuleItem
    {
        public int AppBookCatalogModuleItemID { get; set; }

        public Guid AppID { get; set; }

        public int Sort { get; set; }

        public int MarketBookCatalogID { get; set; }

        public int ModuleID { get; set; }



        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }

        public string BeforeBuyingImg { get; set; }

        public string BeforeBuyingClickImg { get; set; }

        public string BuyLaterClickImg { get; set; }

        public string BuyLaterImg { get; set; }
        public string ModuleName { get; set; }

      

        public int Status { get; set; }

        public int ModelID { get; set; }

        public int ParentModuleID { get; set; }

        /// <summary>
        /// 试看类型:1单元2页码
        /// </summary>
        public int FreeType { get; set; }

        public int FreeNum { get; set; }
    }

    public class RV_AppBookCatalogModuleItem : V_AppBookCatalogModuleItem
    {

        /// <summary>
        /// 是否已购买:1是0未购买
        /// </summary>
        public int HasBuy { get; set; }
        /// <summary>
        /// 是否收费：1是，0不收费
        /// </summary>
        public int IsCharge { get; set; }

        public List<RV_AppBookCatalogModuleItem> Children { get; set; }

    }
}
