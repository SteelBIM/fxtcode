using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.UserOrder.Contract.DataModel
{
    public class UserModuleRequest : Request
    {
        /// <summary>
        /// 市场书籍名
        /// </summary>
        public string MarketBookName { get; set; }

        public List<int> MarketClassifyIdList { get; set; }
        public string UserPhone { get; set; }
    }
}
