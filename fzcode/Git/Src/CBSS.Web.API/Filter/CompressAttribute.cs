using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#pragma warning disable CS0105 // “System”的 using 指令以前在此命名空间中出现过
using System;
#pragma warning restore CS0105 // “System”的 using 指令以前在此命名空间中出现过
#pragma warning disable CS0105 // “System.Collections.Generic”的 using 指令以前在此命名空间中出现过
using System.Collections.Generic;
#pragma warning restore CS0105 // “System.Collections.Generic”的 using 指令以前在此命名空间中出现过
using System.IO;
#pragma warning disable CS0105 // “System.Linq”的 using 指令以前在此命名空间中出现过
using System.Linq;
#pragma warning restore CS0105 // “System.Linq”的 using 指令以前在此命名空间中出现过
#pragma warning disable CS0105 // “System.Web”的 using 指令以前在此命名空间中出现过
using System.Web;
#pragma warning restore CS0105 // “System.Web”的 using 指令以前在此命名空间中出现过
using System.Web.Http.Filters;
using System.IO.Compression;
using System.Net.Http;
namespace CourseActivate.Web.API.Filter
{  
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CompressAttribute”的 XML 注释
    public class CompressAttribute : ActionFilterAttribute
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CompressAttribute”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CompressAttribute.OnActionExecuted(HttpActionExecutedContext)”的 XML 注释
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CompressAttribute.OnActionExecuted(HttpActionExecutedContext)”的 XML 注释
        {
            var content = actionExecutedContext.Response.Content;
            var acceptEncoding = actionExecutedContext.Request.Headers.AcceptEncoding.Where(x => x.Value == "gzip" || x.Value == "deflate").ToList();
            if (acceptEncoding != null && acceptEncoding.Count > 0 && content != null && actionExecutedContext.Request.Method != HttpMethod.Options)
            {
                var bytes = content.ReadAsByteArrayAsync().Result;
                if (acceptEncoding.FirstOrDefault().Value == "gzip")
                {
                    actionExecutedContext.Response.Content = new ByteArrayContent(CompressionHelper.GzipCompress(bytes));
                    actionExecutedContext.Response.Content.Headers.Add("Content-Encoding", "gzip");
                    actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");
                }
                else if (acceptEncoding.FirstOrDefault().Value == "deflate")
                {
                    actionExecutedContext.Response.Content = new ByteArrayContent(CompressionHelper.DeflateCompress(bytes));
                    actionExecutedContext.Response.Content.Headers.Add("Content-encoding", "deflate");
                    actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");
                }
            }
            base.OnActionExecuted(actionExecutedContext);
        }

    }
    class CompressionHelper
    {

        public static byte[] DeflateCompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (DeflateStream gZipStream = new DeflateStream(stream, CompressionMode.Compress))
                    {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();
                    }
                    return stream.ToArray();
                }
            }
            catch (Exception)
            {
                return data;
            }
        }

        public static byte[] GzipCompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress))
                    {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();
                    }
                    return stream.ToArray();
                }
            }
            catch (Exception)
            {
                return data;
            }

        }
    }
}