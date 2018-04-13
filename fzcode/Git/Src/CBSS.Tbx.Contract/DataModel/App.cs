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
    [Table("App")]
    public class App
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [Required(ErrorMessage = "应用名称不能为空")]
        [StringLength(18, ErrorMessage = "应用名称长度不能超过18位")]
        public string AppName { get; set; } 
        /// <summary>
        /// 皮肤ID
        /// </summary>

        public int SkinID { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200, ErrorMessage = "描述长度不能超过200")]
        public string Describe { get; set; }
        /// <summary>
        /// 状态 0未启用1启用2禁用
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
