using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class EntranceApi
    {
        public static object Entrance(string methodName, string methodValue, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            object obj= fxtApi.FxtApi.Entrance(WcfCheck.GetWcfCheckValidDate(), WcfCheck.GetWcfCheckValidCode(),"D", methodName, methodValue);
            fxtApi.Abort();
            return obj;
        }
        public static object Entrance_FxtSpider(string methodName, string methodValue, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            object obj = fxtApi.FxtApi.Entrance(WcfCheck.GetWcfCheckValidDate(), WcfCheck.GetWcfCheckValidCode(), "B", methodName, methodValue);
            fxtApi.Abort();
            return obj;
        }
    }
}
