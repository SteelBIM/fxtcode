using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_pushmessage
    {
        public int? pushid { get; set; }
        public int? activatetypeid { get; set; }
        public string activatetypename { get; set; }
        public string message { get; set; }
        public DateTime? createtime { get; set; }
        public int? hasread { get; set; }
        /// <summary>
        /// 给谁提示 0代表给所有人
        /// </summary>
        public int? towho { get; set; }

        public DateTime? readdate { get; set; }
        public string readmaster { get; set; }
    }
}
