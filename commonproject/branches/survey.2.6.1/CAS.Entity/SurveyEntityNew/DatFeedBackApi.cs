using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.SurveyEntityNew
{
    public class DatFeedBackApi
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 消息时间 
        /// </summary>
        public DateTime msgTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgType { get; set; }
        /// <summary>
        /// 发送者头像
        /// </summary>
        public string senderAvatar { get; set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public int senderId { get; set; }
        /// <summary>
        /// 发送者姓名
        /// </summary>
        public string senderName { get; set; }
    }
}
