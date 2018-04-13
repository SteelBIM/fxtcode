using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
   public class VoteModel
    {
       /// <summary>
       /// 投票 人
       /// </summary>
       public string VoterId { get; set; }
       /// <summary>
       /// 被投票的人
       /// </summary>
       public string UserId { get; set; }
    }
}
