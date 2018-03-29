namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// done结果页实体
    /// </summary>
    public class DoneInfo
    {
        /// <summary>
        /// done结果页的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 自动跳转到的url
        /// </summary>
        public string RefreshUrl { get; set; }

        /// <summary>
        /// 延迟多少秒后跳转
        /// </summary>
        public int LazyTime { get; set; }

        /// <summary>
        /// 自定义跳转链接(key：显示的文本，value：url)
        /// </summary>
        public IDictionary<string, string> Links { get; set; }

        /// <summary>
        /// 显示的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息栏icon
        /// </summary>
        public string MsgIcon { get; set; }
       

        public DoneInfo()
        {
            Links = new Dictionary<string, string>();
        }
    }
}
