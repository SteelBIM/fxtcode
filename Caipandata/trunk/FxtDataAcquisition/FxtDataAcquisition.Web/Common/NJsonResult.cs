namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// 自定义返回json类型
    /// </summary>
    public class NJsonResult : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="handling">是否序列化关联属性</param>
        public NJsonResult(object data, ReferenceLoopHandling handling = ReferenceLoopHandling.Serialize)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            List<JsonConverter> jcs = new List<JsonConverter>(){
                timeFormat
            };

            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            ContentType = "application/json";

            Settings = new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = handling,
                Converters = jcs
            }; 
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Data != null)
            {
                //var data = JsonHelp.ToJSONjss(Data);

                if (ContentEncoding != null)
                {
                    context.HttpContext.Response.ContentEncoding = ContentEncoding;
                }
                
                var scriptSerializer = JsonSerializer.Create(this.Settings);

                using (var sw = new System.IO.StringWriter())
                {
                    scriptSerializer.Serialize(sw, this.Data);
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.Write(sw.ToString());
                }
                //context.HttpContext.Response.Write(data);
                //base.ExecuteResult(context);
            }
        }
    }
}