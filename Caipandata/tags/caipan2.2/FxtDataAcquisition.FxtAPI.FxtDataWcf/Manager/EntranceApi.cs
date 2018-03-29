using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class EntranceApi
    {
        public static object Entrance(string methodName, string methodValue, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            object obj = fxtApi.FxtApi.Entrance(WcfCheck.GetWcfCheckValidDate(), WcfCheck.GetWcfCheckValidCode(), "D", methodName, methodValue);
            fxtApi.Abort();
            return obj;
        }
    }
}
