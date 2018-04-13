using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IYXHearResourceService
    {
        //IEnumerable<v_ListSubModules_0> GetHearResource(int bookId, int catalogId);
        IEnumerable<v_SecondModule> GetYXHearResource(v_ModuleInfo param, List<v_ListSubModules_0> hearResources);
        IEnumerable<TB_HearResources_YX> GetYXUserHearResourcesList(string where);
        bool AddYXResources(TB_HearResources_YX tB_UserHearResources);
        void GetYXUserHearResourcesInfo(v_ModuleInfo submitData, List<v_ListenSpeakInfo> hr, out int finishedTimes);
        List<HearResourceInfoModel> GetYXHearResourceByWeb(string UserID, int BookID, int FirstTitleID, int SecondTitleID, List<v_ListSubModules_0> list);
    }
}
