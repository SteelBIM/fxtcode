using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CAS.Entity.GJBEntity
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class MessageCore
    {
        /// <summary>
        /// 用户ID，多个用","逗号隔开
        /// </summary>
        public string touserids { get; set; }
        /// <summary>
        /// 公司ID，注意不是分支机构ID
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 由谁发送的该消息
        /// </summary>
        public int fromuserid { get; set; }
        public NodeMessage nodemessage;
        public WebChatMessage webchatmessage;
        public ShortMessage shortmessage;
    }
}
