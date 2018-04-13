using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_devicetype
    {
        /// <summary>
        /// 应用类型ID
        /// </summary>
        public int devicetypeid { get; set; }
        /// <summary>
        /// 应用类型名称
        /// </summary>
        public string devicetype { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createTime { get; set; }
    }
}
