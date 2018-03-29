using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Common
{
    [Serializable]
    public class AjaxResult
    {
        public AjaxResult()
        {
        }
        public AjaxResult(string msg, string @goto = "", object data = null)
        {
            Message = msg;
            Result = true;
            Code = "200";
            Goto = @goto;
            Data = data;
        }
        public AjaxResult(string code, string msg)
        {
            Message = msg;
            Code = code;
        }
        [JsonProperty(PropertyName = "code")]
        /// <summary>
        ///  操作结果代码
        /// </summary>
        public string Code { get; set; }

        [JsonProperty(PropertyName = "result")]
        public bool Result { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// 跳转到url
        /// </summary>
        [JsonProperty(PropertyName = "goto")]
        public string Goto { get; set; }
    }
}
