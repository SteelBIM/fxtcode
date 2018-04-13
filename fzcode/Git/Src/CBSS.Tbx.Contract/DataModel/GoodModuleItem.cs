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
    /// 策略配置模块
    /// </summary>
    [Auditable]
    [Table("GoodModuleItem")]
    public class GoodModuleItem
    {
        /// <summary>
        /// 策略配置模块ID
        /// </summary>
        public int GoodModuleItemID { get; set; }

      
        public int GoodID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
       
        /// <summary>
        /// 模块功能
        /// </summary>
        public int ModuleFunctionID { get; set; }
    }
}
