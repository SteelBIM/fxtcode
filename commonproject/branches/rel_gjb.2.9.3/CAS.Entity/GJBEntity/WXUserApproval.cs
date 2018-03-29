using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    /// <summary>
    /// 微信审批请求数据封装类
    /// 潘锦发 20151202
    /// </summary>
    public class WXUserApproval : Dat_NWorkToDo
    {
        /// <summary>
        /// 数据主键编号
        /// </summary>
        [SQLReadOnly]
        public long keyno { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime createdate { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [SQLReadOnly]
        public string number { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string statustpye { get; set; }
        /// <summary>
        /// 询价类型
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        public string projectfullname { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        [SQLReadOnly]
        public string biztype { get; set; }
        /// <summary>
        /// 委托客户
        /// </summary>
        [SQLReadOnly]
        public string bankname { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        [SQLReadOnly]
        public int? businessuserid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [SQLReadOnly]
        public int? statecode { get; set; }
        /// <summary>
        /// 转交状态
        /// </summary>
        [SQLReadOnly]
        public int? assignstate { get; set; }
        /// <summary>
        /// 回价人
        /// </summary>
        [SQLReadOnly]
        public int? appraisersuserid { get; set; }
        /// <summary>
        /// 回价人名称
        /// </summary>
        [SQLReadOnly]
        public string appraisersusername { get; set; }
        /// <summary>
        /// 回价时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? biddate { get; set; }
        /// <summary>
        /// 分配处理人id
        /// </summary>
        [SQLReadOnly]
        public string assinguserids { get; set; }
        /// <summary>
        /// 分配处理人名称
        /// </summary>
        [SQLReadOnly]
        public string assignusername { get; set; }
        /// <summary>
        /// 撰写人id
        /// </summary>
        [SQLReadOnly]
        public int? writerid { get; set; }
        /// <summary>
        /// 撰写人名称
        /// </summary>
        [SQLReadOnly]
        public string writeusername { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        [SQLReadOnly]
        public string buildingarea { get; set; }
        /// <summary>
        /// 评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal? totalprice { get; set; }
        /// <summary>
        /// 总税费
        /// </summary>
        [SQLReadOnly]
        public decimal? tax { get; set; }
        /// <summary>
        /// 总净值
        /// </summary>
        [SQLReadOnly]
        public decimal? netprice { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? completetime { get; set; }
        /// <summary>
        /// 备注nodetype
        /// </summary>
        [SQLReadOnly]
        public string remark { get; set; }
        /// <summary>
        /// 报告业务状态  业务类型的 细分类型null 无 1、房产 2、资产
        /// </summary>
        [SQLReadOnly]
        public int? reporttypecode { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        [SQLReadOnly]
        public int? nodetype { get; set; }
    }
}
