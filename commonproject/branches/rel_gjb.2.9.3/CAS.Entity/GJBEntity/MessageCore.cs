namespace CAS.Entity.GJBEntity
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class MessageCore
    {
        private string _touserids;
        /// <summary>
        /// 用户ID，多个用","逗号隔开
        /// </summary>
        public string touserids
        {
            get
            {
                return _touserids;
            }
            set
            {
                _touserids = value;
                _tousernames = "";
            }
        }
        private string _tousernames;
        /// <summary>
        /// 用户登陆账号，多个用","逗号隔开
        /// </summary>
        public string tousernames {
            get
            {
                return _tousernames;
            }
            set
            {
                _tousernames = value;
                _touserids = "";
            }
        }
        /// <summary>
        /// 公司ID，注意不是分支机构ID
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 由谁发送的该消息
        /// </summary>
        public int fromuserid { get; set; }
        /// <summary>
        /// 由谁发送的该消息    username，处理数据中心的唯一性
        /// </summary>
        public string fromusername { get; set; }
        public NodeMessage nodemessage;
        public WebChatMessage webchatmessage;
        public ShortMessage shortmessage;
    }
}
