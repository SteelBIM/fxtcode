using Kingsun.InterestDubbingGame.Common;
using Kingsun.InterestDubbingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.DAL
{
    public class TB_InterestDubbingGame_MatchDAL : InterestDubbingBaseManagement
    {
        public  List<TB_InterestDubbingGame_UserContentsRecord> GetUserContentRecord(string userId)
        {
            IList<TB_InterestDubbingGame_UserContentsRecord> records = Search<TB_InterestDubbingGame_UserContentsRecord>(" UserID=" + userId);

            if (records == null)
            {
                return new List<TB_InterestDubbingGame_UserContentsRecord>();
            }
            else
            {
                return records.ToList();
            }
        }
    }
}
