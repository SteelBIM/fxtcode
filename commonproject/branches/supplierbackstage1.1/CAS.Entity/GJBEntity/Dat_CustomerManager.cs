using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_CustomerManager : DatCustomerManager
    {
        /// <summary>
        /// 业务员名称
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }

        /// <summary>
        /// 业务员手机号码
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }

        /// <summary>
        /// 客户公司
        /// </summary>
        [SQLReadOnly]
        public string customercompanyname { get; set; }

        /// <summary>
        /// 客户公司分支机构
        /// </summary>
        [SQLReadOnly]
        public string branchname { get; set; }

        /// <summary>
        /// 评估机构ID
        /// </summary>
        [SQLReadOnly]
        public int danweiid { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        [SQLReadOnly]
        public int ywid { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        [SQLReadOnly]
        public int ywvalid { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        [SQLReadOnly]
        public int userstatus { get; set; }

        /// <summary>
        /// 是否内部账号
        /// </summary>
        [SQLReadOnly]
        public int isinside { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 微信发送消息appid
        /// </summary>
        [SQLReadOnly]
        public string appid { get; set; }

        /// <summary>
        ///  微信发送消息appsecret
        /// </summary>
        [SQLReadOnly]
        public string appsecret { get; set; }

        /// <summary>
        ///  银行ID
        /// </summary>
        [SQLReadOnly]
        public int customercompanyid { get; set; }

        /// <summary>
        ///  支机构ID
        /// </summary>
        [SQLReadOnly]
        public int banksubbranchid { get; set; }

        /// <summary>
        ///  分机构ID
        /// </summary>
        [SQLReadOnly]
        public int bankbranchid { get; set; }

        /// <summary>
        ///  支机构ID
        /// </summary>
        [SQLReadOnly]
        public string banksubbranchname { get; set; }

        /// <summary>
        /// 客户公司支行机构
        /// </summary>
        [SQLReadOnly]
        public string subbranchname { get; set; }
        /// <summary>
        /// 机构级别：1和2
        /// </summary>
        [SQLReadOnly]
        public int branchcompanyattr { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        [SQLReadOnly]
        public int provinceid { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [SQLReadOnly]
        public string bumenname { get; set; }

        /// <summary>
        /// 客户公司类型
        /// </summary>
        [SQLReadOnly]
        public string codename { get; set; }
    }
}
