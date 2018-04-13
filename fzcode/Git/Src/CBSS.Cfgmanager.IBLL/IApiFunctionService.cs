using CBSS.Cfgmanager.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.IBLL
{
    public interface IApiFunctionService
    {
        bool SaveApiFunction(Sys_ApiFunction apiFun, List<Sys_ApiFunctionParam> param);
        Sys_ApiFunction GetApiFunction(int ApiFunctionID);
        List<Sys_ApiFunction> GetApiFunctionList(string ApiFunctionIDs);
        List<Sys_ApiFunction> GetApiFunctionList(string ApiFunctionName, int SystemCode);
        List<Sys_ApiFunctionParam> GetApiFunctionParamList(string ApiFunctionIDs);
        List<Sys_ApiFunctionParam> GetApiFunctionParam(int ApiFunctionID);
        IEnumerable<Sys_ApiFunction> GetAllSys_ApiFunction(out int totalcount, ApiFunctionRequest request = null);
        bool DeleteApiFunction(List<int> ids);
    }
}
