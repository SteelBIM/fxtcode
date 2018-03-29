using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.Models
{
    public partial class SYS_UserInfo
    {
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像路径
        /// </summary>
        public string IconUrl { get; set; }
        public int Valid { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
