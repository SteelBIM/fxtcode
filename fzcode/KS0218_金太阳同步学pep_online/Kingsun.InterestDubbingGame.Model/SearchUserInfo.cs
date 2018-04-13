using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    public class SearchUserInfo : TB_InterestDubbingGame_UserInfo
    {
        public string Stage { get; set; }
        public string TotalScore { get; set; }
        /// <summary>
        /// 配音作品
        /// </summary>
        public string DubbingAddress { get; set; }
        /// <summary>
        /// 朗读作品
        /// </summary>
        public string ReadAddress { get; set; }
    }
}
