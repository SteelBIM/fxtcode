using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_batchactivateusedevice
    {
        public int? activateusedeviceid { get; set; }
        public Guid? activateuseid { get; set;}
        public string devicecode{ get; set;}
        /// <summary>
        /// 设备类型，1pc端 2 移动端
        /// </summary>
        public int? devicetype { get; set; }

        public int? isios { get; set; }
        public DateTime? createtime{get; set;}
    }
    public class tb_batchactivateusedevice_copy
    {
        public int activateusedeviceid { get; set; }
        public Guid activateuseid { get; set; }
        public string devicecode { get; set; }
        /// <summary>
        /// 设备类型，1pc端 2 移动端
        /// </summary>
        public int devicetype { get; set; }

        public int isios { get; set; }
        public DateTime createtime { get; set; }
    }

}
