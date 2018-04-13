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
    /// <summary>
    /// 商品
    /// </summary>
    [Auditable]
    [Table("Good")]
    public class Good
    {
        public int GoodID { get; set; }
        /// <summary>
        /// 策略名称
        /// </summary>
        [Required(ErrorMessage = "策略名称不能为空")]
        [StringLength(18, ErrorMessage = "策略名称长度不能超过18位")]
        public string GoodName { get; set; }
        /// <summary>
        /// 商品出售方式1单册2套餐
        /// </summary>
        public int GoodWay { get; set; }
        public int Status { get; set; }

        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200, ErrorMessage = "描述长度不能超过200")]
        public string Describe { get; set; }

    }
}
