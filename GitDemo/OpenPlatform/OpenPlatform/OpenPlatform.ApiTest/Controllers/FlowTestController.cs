using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using OpenPlatform.Framework.FlowMonitor;
using System.Diagnostics;

namespace OpenPlatform.ApiTest.Controllers
{

    public class FlowTestController : ApiController
    {
        public IEnumerable<string> Get()
        {

            var data = new[] { "value1", "value2" };

            var isOverflow = Api.Flow.Overflow(13, DateTime.Now, ApiType.Project, 1003038);

            return isOverflow ? new[] { "流量溢出" } : data;

            //using (var client = new HttpClient())
            //{
            //    var baseAddress = "http://192.168.0.122:8081/";
            //    client.BaseAddress = new Uri(baseAddress);

            //    var result =
            //        client.GetAsync("api/flowmonitor?companyId=25&apiType=1").Result.Content.ReadAsStringAsync().Result;

            //}

            return null;
        }

        public int Get_ForPost(int num)
        {
            using (var client = new HttpClient())
            {
                var baseAddress = "http://192.168.0.122:8081";
                client.BaseAddress = new Uri(baseAddress);

                var requestJson = JsonConvert.SerializeObject(
                    new
                    {
                        CompanyId = 27,
                        InvokeTime = DateTime.Now,
                        ApiType = 1,
                        DataItem = -1,
                        Ip = "",
                        FunctionName = ""
                    });

                HttpContent httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result =
                    client.PostAsync("api/flowmonitor", httpContent).Result.Content.ReadAsStringAsync().Result;
            }

            return 1;
        }
        public bool GetTestFlowOverflow()
        {
            bool isOverflow = false;
            int fxtCompanyId = 25;
            ApiType apiType = ApiType.Case;
            int itemCount = -1;
            string functionName = "gcaseinfooffice";
            int productTypeCode = 1003301;
            string ip = "127.0.0.1";
            //request 2 times
            isOverflow = Api.Flow.Overflow(fxtCompanyId, DateTime.Now, apiType, itemCount, ip, functionName: functionName, productTypeCode: productTypeCode);
            isOverflow = Api.Flow.Overflow(fxtCompanyId, DateTime.Now, apiType, itemCount, ip, functionName: functionName, productTypeCode: productTypeCode);
            return isOverflow;
        }
    }
}
