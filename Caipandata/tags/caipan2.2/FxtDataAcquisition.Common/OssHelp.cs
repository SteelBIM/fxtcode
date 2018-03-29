using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Common
{
    public class OssHelp
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">绝对路径</param>
        /// <param name="relatePath">相对路径</param>
        /// <returns></returns>
        public static async Task<Tuple<bool,string>> UpFileAsync(string filePath,string relatePath)
        {
            using (var fs=File.OpenRead(filePath))//读取文件
            using (HttpClient client = new HttpClient())
            using (StreamContent content = new StreamContent(fs))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["OssUpload"]);
                var cache = new CacheControlHeaderValue();
                cache.MaxAge = new TimeSpan(1, 0, 0);
                cache.MustRevalidate = true;
                client.DefaultRequestHeaders.CacheControl = cache;
                var result = await client.PostAsync("api/oss?f=" + relatePath, content);
                if (result.IsSuccessStatusCode)
                {
                    return new Tuple<bool, string>(true,null);
                }
                else
                {
                    var err = "上传文件错误：" + await result.Content.ReadAsStringAsync();
                    return new Tuple<bool, string>(false, err);
                }
            }

        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">文件流</param>
        /// <param name="relatePath">相对路径</param>
        /// <returns></returns>
        public static async Task<Tuple<bool, string>> UpFileAsync(Stream file, string relatePath)
        {
            using (HttpClient client = new HttpClient())
            using (StreamContent content = new StreamContent(file))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["OssUpload"]);
                var cache = new CacheControlHeaderValue();
                cache.MaxAge = new TimeSpan(1, 0, 0);
                cache.MustRevalidate = true;
                client.DefaultRequestHeaders.CacheControl = cache;
                var result = await client.PostAsync("api/oss?f=" + relatePath, content);
                if (result.IsSuccessStatusCode)
                {
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    var err = "上传文件错误：" + await result.Content.ReadAsStringAsync();
                    return new Tuple<bool, string>(false, err);
                }
            }

        }
    }
}
