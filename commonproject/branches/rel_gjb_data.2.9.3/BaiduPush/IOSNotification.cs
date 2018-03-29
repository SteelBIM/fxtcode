using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BaiduPush
{
    public class IOSNotification
    {
        public string title { get; set; } //通知标题，可以为空；如果为空则设为appid对应的应用名;
        public string description { get; set; } //通知文本内容，不能为空;
        public string aps { get; set; }
        public int notification_builder_id { get; set; }
        public int notification_basic_style { get; set; }
        public int open_type { get; set; }
        public string  url { get; set; }
        public int user_confirm { get; set; }
        public string pkg_content { get; set; }

        public IOSNotification()
        {
        }

        public string getJsonString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}