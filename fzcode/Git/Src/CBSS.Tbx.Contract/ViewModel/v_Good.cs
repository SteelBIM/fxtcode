using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_Good
    {
        public int GoodID { get; set; }
        /// <summary>
        /// 策略名称
        /// </summary>

        public string GoodName { get; set; }
        /// <summary>
        /// 商品出售方式1单册2套餐
        /// </summary>
        public int GoodWay { get; set; }
        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModuleNames { get; set; }
        public string Describe { get; set; }
    }
}
