using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace FxtCenterService.Common
{
    public class OssHelp
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">绝对路径</param>
        /// <param name="relatePath">相对路径</param>
        /// <returns></returns>
        public static bool UpFileAsync(string filePath, string relatePath)
        {
            using (var fs = File.OpenRead(filePath))//读取文件
            using (HttpClient client = new HttpClient())
            using (StreamContent content = new StreamContent(fs))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["OssUpload"]);
                var cache = new CacheControlHeaderValue();
                cache.MaxAge = new TimeSpan(1, 0, 0);
                cache.MustRevalidate = true;
                client.DefaultRequestHeaders.CacheControl = cache;
                var result = client.PostAsync("api/oss?f=" + relatePath, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var err = "上传文件错误：" + result.Result.Content.ReadAsStringAsync();
                    return false;
                }
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">文件流</param>
        /// <param name="relatePath">相对路径</param>
        /// <returns></returns>
        public static bool UpFileAsync(Stream file, string relatePath)
        {
            using (HttpClient client = new HttpClient())
            using (StreamContent content = new StreamContent(file))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["OssUpload"]);
                var cache = new CacheControlHeaderValue();
                cache.MaxAge = new TimeSpan(1, 0, 0);
                cache.MustRevalidate = true;
                client.DefaultRequestHeaders.CacheControl = cache;
                var result = client.PostAsync("api/oss?f=" + relatePath, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var err = "上传文件错误：" + result.Result.Content.ReadAsStringAsync();
                    return false;
                }
            }

        }
    }
}
