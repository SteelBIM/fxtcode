using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.CASEntity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class CASLoginEntity : BaseTO
    {
        /// <summary>
        /// 客户账号
        /// </summary>
        public  string userName { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string trueName { get; set; }
        /// <summary>
        /// 客户公司ID
        /// </summary>
        public int customercompanyid { get; set; }
        /// <summary>
        /// 客户公司名称
        /// </summary>
        public string customercompanyname{ get; set; }
        /// <summary>
        /// 分公司ID
        /// </summary>
        public int banksubbranchid { get; set; }
        /// <summary>
        /// 部门/支行ID
        /// </summary>
        public int bankbranchid { get; set; }
        /// <summary>
        /// 客户对接业务员
        /// </summary>
        public int? businessuserid { get; set; }
        /// <summary>
        /// 客户手机号码
        /// </summary>
        public string telphone { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int customerid { get; set; }
        /// <summary>
        /// 分支机构/部门
        /// </summary>
        public int subcompanyid { get; set; }
        /// <summary>
        /// 分支机构/部门
        /// </summary>
        public string bumenname { get; set; }
        /// <summary>
        /// 分公司名称
        /// </summary>
        public string branchname { get; set; }
        /// <summary>
        /// 部门/支行名称
        /// </summary>
        public string banksubbranchname { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid { get; set; }
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid { get; set; }
        /// <summary>
        /// 公司标示
        /// </summary>
        public string signname { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string wxopenid { get; set; }
        /// <summary>
        /// 客户公司类型
        /// </summary>
        public string codename { get; set; }
        /// <summary>
        /// 业务员联系电话
        /// </summary>
        public string businessuseridmobile { get; set; }
        /// <summary>
        /// 对接业务部门ID
        /// </summary>
        public int departmentid { get; set; }
        /// <summary>
        /// 数据查看权限  20150610 潘锦发
        /// </summary>
        public string permissionSee { get; set; }
        /// <summary>
        /// App数组
        /// </summary>
        public Dictionary<string, AppArray> apps { get; set; }
    }
}
