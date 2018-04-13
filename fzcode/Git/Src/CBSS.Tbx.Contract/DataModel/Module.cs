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
    [Auditable]
    [Table("Module")]
    public class Module
    {
        public int ModuleID { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        [Required(ErrorMessage = "模块名称不能为空")]
        [StringLength(36, ErrorMessage = "模块名称长度不能超过36位")]
        public string ModuleName { get; set; }

        [Required(ErrorMessage = "模型ID")]
        public int ModelID { get; set; }

        [Required(ErrorMessage = "书籍ID")]
        public int MarketBookID { get; set; }

        /// <summary>
        /// 资源访问方式  1 新MOD  2 本地 3 第三方
        /// </summary>
        public int SourceAccessMode { get; set; }

        /// <summary>
        /// 试看类型 1 单元  2 页码
        /// </summary>
        public int FreeType { get; set; }

        /// <summary>
        /// 上级模块ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 试看数量
        /// </summary>
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "请输入非负正整数")]
        //[Range(0, 10000,ErrorMessage = "请输入非负正整数")]
        public int FreeNum { get; set; }

        //[Required(ErrorMessage = "模块图片不能为空")]
        //public string ModuleImg { get; set; } 
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

    }
}
