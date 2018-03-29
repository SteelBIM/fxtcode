using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

/***
 * 作者:  李晓东
 * 时间:  2013.12.5
 * 摘要:  创建 HttpClientHelper HttpClient帮助类
 * **/
namespace FxtCollateralManager.Common
{
    public class HttpClientHelper
    {
        protected static HttpClient httpClient = null;
        public HttpClientHelper()
        {
            httpClient = new HttpClient();
        }
        
        public static string GetAsync(string url)
        {
            //var postData = new List<KeyValuePair<string, string>>();
            //postData.Add(new KeyValuePair<string, string>("", "1"));
            //postData.Add(new KeyValuePair<string, string>("aa", "2"));
            //var content = new FormUrlEncodedContent(postData);
            ////Post
            //var response = httpClient.PostAsync(GetUrl("api/Test/Posts/"), null);

            //string aa = response.Result.Content.ReadAsStringAsync().Result;
            //Get
            var result = httpClient.GetAsync(url).Result;
            return result.Content.ReadAsAsync<string>().Result;
        }
    }
}
