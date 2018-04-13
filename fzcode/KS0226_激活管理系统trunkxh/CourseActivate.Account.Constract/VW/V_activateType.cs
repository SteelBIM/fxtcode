using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    [Serializable]
    /// <summary>
    /// 激活码类型设置视图
    /// </summary>
    public class V_activateType
    {
        public int activatetypeid { get; set; }
        public string activatetypename { get; set; }
        public int publishid { get; set; }
        /// <summary>
        /// 设备类型，0全部,1pc端 2 移动端
        /// </summary>
        public int? type { get; set; }
        public int? way { get; set; }
        public int devicenum { get; set; }
        public int? status { get; set; }
        public string remark { get; set; }
        public DateTime? createTime { get; set; }

        public string publishname { get; set; }
    }
}
