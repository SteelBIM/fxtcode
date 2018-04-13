using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IHearResourceService
    {
        //IEnumerable<v_ListSubModules_0> GetHearResource(int bookId, int catalogId);
        IEnumerable<v_SecondModule> GetHearResource(v_ModuleInfo param, List<v_ListSubModules_0> hearResources);
        IEnumerable<TB_HearResources> GetUserHearResourcesList(string where);
        bool AddResources(TB_HearResources tB_UserHearResources);
        void GetUserHearResourcesInfo(v_ModuleInfo submitData, List<v_ListenSpeakInfo> hr, out int finishedTimes);
        List<HearResourceInfoModel> GetHearResourceByWeb(string UserID, int BookID, int FirstTitleID, string SecondTitleID, List<v_ListSubModules_0> list);
    }
}
