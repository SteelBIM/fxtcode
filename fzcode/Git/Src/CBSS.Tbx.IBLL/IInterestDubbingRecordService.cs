using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.Contract.API;
using CBSS.Tbx.Contract.ViewModel;

namespace CBSS.Tbx.IBLL
{
    public interface IInterestDubbingRecordService
    {
        APIResponse GetDubbingByCataId(v_BookInfo input);
        APIResponse GetVideoList(string appId, int PageIndex, int State, int userId, int isEnableOss);
        APIResponse UpdateVedioInfo(string state, string idStr);
        APIResponse InsertVideoInfo(UserVideoInfo input);
        APIResponse SchoolRankInfo(UserSchoolRankParaModel input);
        APIResponse ClassRankInfo(UserSchoolRankParaModel input);
        APIResponse NewRankInfo(UserSchoolRankParaModel input);
        APIResponse VideoRankingInfo(UserVideoDetails input);
        APIResponse VideoAchievementInfo(VideoAchievement input);
    }
}
