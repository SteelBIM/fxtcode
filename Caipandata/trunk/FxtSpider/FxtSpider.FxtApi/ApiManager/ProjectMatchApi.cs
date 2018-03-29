using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Fxt.Api;
using FxtSpider.FxtApi.Model;
using FxtSpider.Common;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class ProjectMatchApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ProjectMatchApi));
        public static bool InsertProjectMatch(List<FxtApi_SYSProjectMatch> list, out string message, FxtAPIClientExtend _fxtApi = null)
        {
            message = "";
            if (list == null || list.Count < 1)
            {
                return true;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                list = list.EncodeField();
                string json = list.ToJSONjss();


                string name = "InsertSYSProjectMatchList";
                var para = new { jsonData = json };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_Result result = FxtApi_Result.ConvertToObj(jsonStr);
                if (result.Result == 0)
                {
                    message = result.Message.DecodeField();
                    fxtApi.Abort();
                    return false;
                }
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("InsertProjectMatch(List<FxtApi_SYSProjectMatch> list,out string message)", ex);
                return false;
            }
            return true;
        }
    }
}
