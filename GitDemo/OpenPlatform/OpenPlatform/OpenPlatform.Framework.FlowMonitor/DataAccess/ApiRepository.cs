using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using OpenPlatform.Framework.FlowMonitor.Models;
using System.Web;

namespace OpenPlatform.Framework.FlowMonitor.DataAccess
{
    class ApiRepository
    {
        private static readonly HttpClient HttpClient;

        static ApiRepository()
        {
            var url = ConfigurationManager.AppSettings["FlowMonitorUrl"];
            //url = "http://192.168.0.37:80";
            HttpClient = new HttpClient() { BaseAddress = new Uri(url) };
            HttpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public static FlowAccessDto GetInvokeLog(int companyId, string invokedDate, int apiType, int productTypeCode, string key1, string key2)
        {
            FlowAccessDto flowAccess = new FlowAccessDto();
            string key = key1 + "_" + key2 + "_temp";
            var obj = HttpRuntime.Cache.Get(key);
            if (obj == null)
            {
                HttpRuntime.Cache.Insert(key,"1");
                var result =
                       HttpClient.GetAsync("api/flowmonitor?companyId=" + companyId + "&invokedDate=" + invokedDate + "&apiType=" + apiType + "&productTypeCode=" + productTypeCode).Result.Content.ReadAsStringAsync().Result;

                flowAccess = new JavaScriptSerializer().Deserialize<FlowAccessDto>(result);

                if (flowAccess == null)
                {
                    //清理昨天缓存
                    Api.Clear(companyId, Convert.ToDateTime(invokedDate), (ApiType)apiType, productTypeCode);
                    HttpRuntime.Cache.Insert(key1, 0);
                    HttpRuntime.Cache.Insert(key2, 0);
                }
                else
                {
                    HttpRuntime.Cache.Insert(key1, flowAccess.AccessedTimes);
                    HttpRuntime.Cache.Insert(key2, flowAccess.TotalDataItems);
                }
                HttpRuntime.Cache.Remove(key);
            }
            else
            {
                flowAccess.AccessedTimes = 0;
                flowAccess.TotalDataItems = 0;
            }
            return flowAccess;
        }

        public static FlowAccessDto GetFlowControlConfig(int companyId, int apiType, int productTypeCode)
        {
            var result =
                  HttpClient.GetAsync("api/flowmonitor?companyId=" + companyId + "&apiType=" + apiType + "&productTypeCode=" + productTypeCode).Result.Content.ReadAsStringAsync().Result;

            var flowAccess = new JavaScriptSerializer().Deserialize<FlowAccessDto>(result);

            return flowAccess;
        }

        public static string AddApiInvokeLog(object jsonApiLog)
        {
            var requestContent = new JavaScriptSerializer().Serialize(jsonApiLog);
            HttpContent httpContent = new StringContent(requestContent);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result =
                    HttpClient.PostAsync("api/flowmonitor", httpContent).Result.Content.ReadAsStringAsync().Result;

            return result;
        }

    }
}
