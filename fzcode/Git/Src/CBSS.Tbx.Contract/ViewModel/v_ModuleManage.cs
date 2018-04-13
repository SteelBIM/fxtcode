using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_ModuleManage
    {
        public int ModuleID { get; set; }
       
        public string ModuleName { get; set; }

       
        public int MarketBookID { get; set; }

        public string ParentModuleName { get; set; }

        /// <summary>
        /// 资源访问方式  1 新MOD  2 本地 3 第三方
        /// </summary>
        public int SourceAccessMode { get; set; }

        /// <summary>
        /// 试看类型 1 单元  2 页码
        /// </summary>
        public int FreeType { get; set; }

       
        public int FreeNum { get; set; }
 
        public string ModuleImg { get; set; }

        public string SourceAddress { get; set; }
        /// <summary>
        /// MOD资源类型，如：1.电子书、2.练习册
        /// </summary>
        public int MODSourceType { get; set; }
        public int Status { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }
        public string ModelName { get; set; }
      
    }
}
