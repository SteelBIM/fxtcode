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
    /// 应用选择模块
    /// </summary>
    [Auditable]
    [Table("AppModuleItem")]
    public class AppModuleItem
    {
        public int AppModuleItemID { get; set; }
        public string AppID { get; set; }
        public int ModuleID { get; set; } 
        /// <summary>
        /// 前端显示名称不能为空
        /// </summary>
        [Required(ErrorMessage = "前端显示名称不能为空")]
        public string ProductShowName { get; set; }
        public int Sort { get; set; }
        /// <summary>
        /// 购买前图片
        /// </summary>
        public string BeforeBuyingImg { get; set; }
        /// <summary>
        /// 购买后图片
        /// </summary>
        public string BuyLaterImg { get; set; }
         
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }

    }
}
