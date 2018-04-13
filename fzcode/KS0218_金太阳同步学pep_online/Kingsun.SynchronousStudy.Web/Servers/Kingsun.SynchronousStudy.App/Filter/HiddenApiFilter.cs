using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Kingsun.SynchronousStudy.App.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]

    public partial class ShowApiAttribute : Attribute { }
    public class ShowApiFilter : IDocumentFilter
    {
        /// <summary>  
        /// 重写Apply方法，移除隐藏接口的生成  
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="schemaRegistry"></param>  
        /// <param name="apiExplorer">api接口集合</param>  
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            IDictionary<string, PathItem> temp = new System.Collections.Generic.Dictionary<string, PathItem>();
            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                if (Enumerable.OfType<ShowApiAttribute>(apiDescription.GetControllerAndActionAttributes<ShowApiAttribute>()).Any())
                {
                    string key = "/" + apiDescription.RelativePath;
                    if (key.Contains("?"))
                    {
                        int idx = key.IndexOf("?", StringComparison.Ordinal);
                        key = key.Substring(0, idx);
                    }
                    temp.Add(key, swaggerDoc.paths[key]);
                }
            }
            swaggerDoc.paths = temp;
        }
    }
}