using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    /// <summary>
    /// 微信请求数据封装类
    /// </summary>
    public class WXUserWorking:BaseTO
    {
        public long id { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public long? entrustid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string statustpye { get; set; }
        /// <summary>
        /// 询价类型
        /// </summary>
        public string querytypename { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectfullname { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        public string biztype { get; set; }
        /// <summary>
        /// 委托客户
        /// </summary>
        public string bankname { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public int? businessuserid { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string businessusername { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? statecode { get; set; }
        /// <summary>
        /// 转交状态
        /// </summary>
        public int? assignstate { get; set; }
        /// <summary>
        /// 回价人
        /// </summary>
        public int? appraisersuserid { get; set; }
        /// <summary>
        /// 回价人名称
        /// </summary>
        public string appraisersusername { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypename { get; set; }
        /// <summary>
        /// 评估目的
        /// </summary>
        public string assesstype { get; set; }
        /// <summary>
        /// 分配处理人id
        /// </summary>
        public string assinguserids { get; set; }
        /// <summary>
        /// 分配处理人名称
        /// </summary>
        public string assignusername { get; set; }
        /// <summary>
        /// 撰写人id
        /// </summary>
        public int? writerid { get; set; }
        /// <summary>
        /// 撰写人名称
        /// </summary>
        public string writeusername { get; set; }
    }
}
