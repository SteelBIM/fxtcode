using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.GJBEntity
{
    public class MessageTemp
    {
        /// <summary>
        /// 业务类型，预评、业务、报告、询价等
        /// </summary>
        public string businessType { get; set; }
        /// <summary>
        /// 发起人
        /// </summary>
        public string fromUserName { get; set; }
        /// <summary>
        /// 完成人
        /// </summary>
        public string toUserName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 委托类型 对公、个人或者询价类型(住宅、商业、办公等)
        /// </summary>
        public string typecodename { get; set; }
        /// <summary>
        /// 消息模板类型
        /// </summary>
        public string messagetype { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal totalprice { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string saleman { get; set; }
        /// <summary>
        /// 业务员联系方式
        /// </summary>
        public string salemantelephone { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal buildarea { get; set; }
    }
}
