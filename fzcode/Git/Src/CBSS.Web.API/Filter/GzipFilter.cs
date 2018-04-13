using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CourseActivate.Web.API.Filter
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“GzipFilter”的 XML 注释
    public class GzipFilter : DelegatingHandler
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“GzipFilter”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“GzipFilter.SendAsync(HttpRequestMessage, CancellationToken)”的 XML 注释
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“GzipFilter.SendAsync(HttpRequestMessage, CancellationToken)”的 XML 注释
        {
            Stream stream = request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            stream.Position = 0;



            if (request.Content.Headers.Contains("Content-Encoding") && request.Content.Headers.GetValues("Content-Encoding").ToArray().FirstOrDefault() == "gzip")
            //     if (contentEncoding. && contentEncoding.Value.ToArray()[0] == "gzip")
            {
                request.Content = new StreamContent(new GZipStream(stream, CompressionMode.Decompress));
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.Add("Content-encoding", "gzip");
                request.Content.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}