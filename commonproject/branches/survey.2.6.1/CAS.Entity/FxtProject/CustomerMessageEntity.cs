using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtProject
{
    public class CustomerMessageEntity : BaseTO
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 消息类型 1:待办 2：消息 3：通知 4: 历史已登录账号推送事件
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 业务类型 1：询价、2：预评、3：报告、4：业务、5：行政审批、6：查勘、7：待归档、8：文件传阅、
        /// 9：群组消息 kevin 20140703
        /// 10：部门内消息 kevin 20140705
        /// </summary>
        public int businesstype { get; set; }
        private int _businesssource = 1;
        /// <summary>
        /// 业务来源 1：系统OA、2：微信；3:CAS
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
        /// Web消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime createdon { get; set; }
        private string _touserids;
        /// <summary>
        /// 发送给用户的用户ID，用户Id请使用个格式如：“user_公司ID_用户ID”，注意是公司ID，不是分支机构ID。实例：“user_365_12” string.format(CAS.Common.ConstCommon.NodeJsSessionId, args1, args2);
        /// 多个用户请使用逗号间隔
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
        /// 发送给用户的用户账号，用户Id请使用个格式如：“user_公司ID_用户账号”，注意是公司ID，不是分支机构ID。实例：“user_365_admin@gjb” string.format(CAS.Common.ConstCommon.NodeJsSessionId, args1, args2);
        /// 多个用户请使用逗号间隔
        /// </summary>
        public string tousernames
        {
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
        public string fromtruename { get; set; }
        /// <summary>
        /// 由谁发送的该消息，业务主体中的发送人
        /// </summary>
        public string fromusername { get; set; }
        /// <summary>
        /// 待办环节 待回价 待分配 待撰写 待审批(细化到审批步骤)
        /// </summary>
        public string stepname { get; set; }
        /// <summary>
        /// 微信消息内容
        /// </summary>
        public string wxmsgcontent  { get; set; }
        /// <summary>
        /// 手机消息内容
        /// </summary>
        public string telephonemsgcontent { get; set; }
        /// <summary>
        /// 微信 AppId
        /// </summary>
        public string appid  { get; set; }
        /// <summary>
        /// 微信 AppSecret
        /// </summary>
        public string appsecret { get; set; }
        /// <summary>
        /// 客户公司加客户名称
        /// </summary>
        public string customerFullName { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }
        
    }

    public enum SendType
    {
        用户消息,
        部门消息,       //机构消息也在此部门消息范围内
        公司消息
    }

    /// <summary>
    /// 消息枚举
    /// </summary>
    public enum MessageEnum 
    {
        /// <summary>
        /// 系统OA,value:int类型
        /// </summary>
        Businesssource_OA = 1,
        /// <summary>
        /// 微信,value:int类型
        /// </summary>
        Businesssource_WX = 2,
        /// <summary>
        /// CAS,value:int类型
        /// </summary>
        Businesssource_CAS = 3,
        // 1：询价、2：预评、3：报告、4：业务、5：行政审批、6：查勘、7：待归档、8：文件传阅
        /// <summary>
        /// 询价,value:int类型
        /// </summary>
        Businesstype_Query = 1,
        /// <summary>
        /// 预评,value:int类型
        /// </summary>
        Businesstype_YP = 2,
        /// <summary>
        /// 报告,value:int类型
        /// </summary>
        Businesstype_Report = 3,
        /// <summary>
        /// 业务,value:int类型
        /// </summary>
        Businesstype_Entrust = 4,
        /// <summary>
        /// 行政审批,value:int类型
        /// </summary>
        Businesstype_Administrative = 5,
        /// <summary>
        /// 待归档,value:int类型
        /// </summary>
        Businesstype_Survey= 7,
        /// <summary>
        /// 待归档,value:int类型
        /// </summary>
        Businesstype_Verified = 7,
        /// <summary>
        /// 文件传阅,value:int类型
        /// </summary>
        Businesstype_Document = 8,
        /// <summary>
        /// 自动估价单盖章,value:int类型
        /// </summary>
        Businesstype_AutoQueryStamp = 12,  
        /// <summary>
        /// 人工询价单盖章,value:int类型
        /// </summary>
        Businesstype_QueryStamp = 13,      
        //消息类型 1:待办 2：消息 3：通知 4: 历史已登录账号推送事件
        /// <summary>
        /// 待办,value:int类型
        /// </summary>
        Type_Wait = 1,
        /// <summary>
        /// 消息,value:int类型
        /// </summary>
        Type_Messge = 2,
        /// <summary>
        /// 通知,value:int类型
        /// </summary>
        Type_Notice = 3,
    }
    /// <summary>
    /// 环节
    /// </summary>
    public class Stepname
    {
        //待办环节 待回价 待分配 待撰写 待审批(细化到审批步骤)
        /// <summary>
        /// 待回价,value:string类型
        /// </summary>
        public const string Stepname_WaitPrice = "待回价";
        /// <summary>
        /// 待分配,value:string类型
        /// </summary>
       public const string Stepname_WaitAllot = "待分配";
        /// <summary>
        /// 待撰写,value:string类型
        /// </summary>
       public const string Stepname_WaitWrite = "待撰写";
        /// <summary>
        /// 待审批,value:string类型
        /// </summary>
       public const string Stepname_WaitApproval = "待审批";
       /// <summary>
       /// 待盖章,value:string类型
       /// </summary>
       public const string Stepname_WaitStamp = "待盖章";
    }
    /// <summary>
    /// 标题
    /// </summary>
    public class Title
    {
        /// <summary>
        /// 询价
        /// </summary>
        public const string Query = "询价";
        /// <summary>
        /// 预评
        /// </summary>
       public const string YP = "预评";
        /// <summary>
       /// 报告
        /// </summary>
       public const string Report = "报告";
    }
}
