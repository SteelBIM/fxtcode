using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Common.Common
{
    public class OssHelper
    {
        public static Tuple<bool, string> UpFileAsync(StreamContent content, string relatePath)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationHelper.OssUri);
                var cache = new CacheControlHeaderValue();
                cache.MaxAge = new TimeSpan(1, 0, 0);
                cache.MustRevalidate = true;
                client.DefaultRequestHeaders.CacheControl = cache;
                var result = client.PostAsync("api/oss?f=" + relatePath, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    var err = "上传文件错误";
                    return new Tuple<bool, string>(false, err);
                }
            }

        }
    }
}
