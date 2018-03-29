using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{

    [Serializable]
    public class Message : BaseTO
    {
        /// <summary>
        /// 消息类型 1:待办 2：消息 3：通知
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 业务类型 1：询价、2：预评、3：报告、4：业务、5：行政审批、6：查勘、7：待归档
        /// </summary>
        public int businesstype { get; set; }
        private int _businesssource = 1;
        /// <summary>
        /// 业务来源 1：系统OA、2：微信
        /// </summary>
        public int businesssource { get { return _businesssource; } set { _businesssource = value; } }
        /// <summary>
        /// 业务Id
        /// </summary>
        public long businessid { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime createdon { get; set; }
        /// <summary>
        /// 由谁发送的该消息，业务主体中的发送人
        /// </summary>
        public string fromusername { get; set; }
        /// <summary>
        /// 待办环节 待回价 待分配 待撰写 待审批(细化到审批步骤)
        /// </summary>
        public string stepname { get; set; }
        /// <summary>
        /// 待办条数总计
        /// </summary>
        public int daibancount { get; set; }
        /// <summary>
        /// 消息条数总计
        /// </summary>
        public int xiaoxicount { get; set; }
        /// <summary>
        /// 通知条数总计
        /// </summary>
        public int tongzhicount { get; set; }
        /// <summary>
        /// 指示是否截取消息内容
        /// 1:为截取(截取的长度取ConstCommon.MaxCharLength) 0:为不截取
        /// </summary>
        public int intercept { get; set; }        
    }

    /// <summary>
    /// NodeJs消息实体
    /// </summary>
    [Serializable]
    public class NodeMessage
    {
        /// <summary>
        /// 消息类型 1:待办 2：消息 3：通知
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 业务类型 1：询价、2：预评、3：报告、4：业务、5：行政审批、6：查勘、7：待归档
        /// </summary>
        public int businesstype { get; set; }
        private int _businesssource = 1;
        /// <summary>
        /// 业务来源 1：系统OA、2：微信
        /// </summary>
        public int businesssource { get { return _businesssource; } set { _businesssource = value; } }
        /// <summary>
        /// 业务Id
        /// </summary>
        public long businessid { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime createdon { get; set; }
        /// <summary>
        /// 发送给用户的用户ID，用户Id请使用个格式如：“user_公司ID_用户ID”，注意是公司ID，不是分支机构ID。实例：“user_365_12” string.format(CAS.Common.ConstCommon.NodeJsSessionId, args1, args2);
        /// 多个用户请使用逗号间隔
        /// </summary>
        public string touserids { get; set; }
        /// <summary>
        /// 发送给部门的消息，"user_公司ID_部门ID"
        /// 多个用户请使用逗号间隔
        /// </summary>
        public string todepartmentids { get; set; }

        /// <summary>
        /// 发送给机构的消息，"user_公司ID"
        /// 多个用户请使用逗号间隔
        /// </summary>
        public string tocompanyids { get; set; }

        /// <summary>
        /// 由谁发送的该消息，业务主体中的发送人
        /// </summary>
        public string fromusername { get; set; }
        /// <summary>
        /// 待办环节 待回价 待分配 待撰写 待审批(细化到审批步骤)
        /// </summary>
        public string stepname { get; set; }

        public string token
        {
            get
            {
                return ConfigurationManager.AppSettings["NodeServiceToken"];
            }
        }
    }

    /// <summary>
    /// 微信消息实体
    /// </summary>
    [Serializable]
    public class WebChatMessage
    {
        public string message { get; set; }        
    }

    /// <summary>
    /// 手机短信实体
    /// </summary>
    public class ShortMessage
    {
        public string message { get; set; }
    }
}
