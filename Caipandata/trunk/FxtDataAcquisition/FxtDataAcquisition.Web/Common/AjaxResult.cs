namespace FxtDataAcquisition.Web.Common
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// ajax请求返回结果
    /// </summary>
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
        /// <summary>
        ///  操作结果代码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        /// <summary>
        /// 操作结果
        /// </summary>

        [JsonProperty(PropertyName = "result")]
        public bool Result { get; set; }
        /// <summary>
        /// 消息
        /// </summary>

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// 跳转到url
        /// </summary>
        [JsonProperty(PropertyName = "goto")]
        public string Goto { get; set; }
    }
}
