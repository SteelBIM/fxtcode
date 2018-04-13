using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("Model")]
    public class Model
    {


        public int ModelID { get; set; }
        /// <summary>
        /// 模型名称
        /// </summary>
        [Required(ErrorMessage = "模型名称不能为空")]
        [StringLength(18, ErrorMessage = "模型名称长度不能超过18个字符")]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "父级模型ID")]
        public int ParentID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的正整数")]
        [Range(1, 10000)]
        public int Sort { get; set; }


        /// <summary>
        /// 老同步学模型ID
        /// </summary>
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的正整数")]
        [Range(1, 1000000)]
        public int OldModelID { get; set; }


        
        /// <summary>
        /// 资源模板地址
        /// </summary>
        public string ResourceTemplate { get; set; }

        /// <summary>
        /// 1 课内模型 2课外模型 
        /// </summary>
        public int ModelType { get; set; }
        /// <summary>
        /// 模型功能 1评测 2跟读
        /// </summary>
        public string FunctionID { get; set; }
        /// <summary>
        /// 资源分类 1:原生，2:H5
        /// </summary>
        public int ModelSourceType { get; set; }
        /// <summary>
        /// H5链接地址
        /// </summary>
        [Required(ErrorMessage = "H5链接地址不能为空")]
        public string H5Url { get; set; }

        /// <summary>
        ///状态 0未启用1启用2禁用
        /// </summary>
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
