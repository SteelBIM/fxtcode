using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.UserOrder.Contract.ViewModel
{
    public class v_UserModuleItem
    {
        public int UserModuleItemID { get; set; }
        public string PayOrderID { get; set; }
        public int UserID { get; set; }
        public int MarketClassifyId { get; set; }
        public string MarketBookName { get; set; }
        public string ModuleName { get; set; }
        /// <summary>
        /// 使用时长
        /// </summary>
        public int Months { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 状态：0未启用 1启用 2禁用
        /// </summary>
        public int States { get; set; }
        public string Remark { get; set; }
        public string UserPhone { get; set; }
       
    }
}
