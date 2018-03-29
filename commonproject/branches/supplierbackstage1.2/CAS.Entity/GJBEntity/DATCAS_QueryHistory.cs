using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class DATCAS_QueryHistory : DATCASQueryHistory
    {
        /// <summary>
        /// 房屋用途
        /// </summary>
        [SQLReadOnly]
        public string purpose { get; set; }
        /// <summary>
        /// 建筑类型
        /// </summary>
        [SQLReadOnly]
        public string buildingtype { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [SQLReadOnly]
        public string housetype { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [SQLReadOnly]
        public string department { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string businessuser { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [SQLReadOnly]
        public string customername { get; set; }
        /// <summary>
        /// 客户单位
        /// </summary>
        [SQLReadOnly]
        public string branchname { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [SQLReadOnly]
        public int departmentid { get; set; }

        /// <summary>
        /// 人工询价面积
        /// </summary>
        [SQLReadOnly]
        public decimal? querybuildingarea { get; set; }
        /// <summary>
        /// 人工询价税费
        /// </summary>
        [SQLReadOnly]
        public decimal? querytax { get; set; }
        /// <summary>
        /// 人工询价净值
        /// </summary>
        [SQLReadOnly]
        public decimal? querynetprice { get; set; }
        /// <summary>
        /// 人工询价单价
        /// </summary>
        [SQLReadOnly]
        public decimal? queryunitprice { get; set; }
        /// <summary>
        /// 人工询价总价
        /// </summary>
        [SQLReadOnly]
        public decimal? querytotalprice { get; set; }
        /// <summary>
        /// 询价类型
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }
        /// <summary>
        /// 询价类型code
        /// </summary>
        [SQLReadOnly]
        public int? querytypecode { get; set; }
        /// <summary>
        /// 询价状态code
        /// </summary>
        [SQLReadOnly]
        public int? statecode { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string salemanname { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }
        /// <summary>
        /// 回价备注
        /// </summary>
        [SQLReadOnly]
        public string priceremark { get; set; }
        /// <summary>
        /// 人工询价回价时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? biddate { get; set; }
        /// <summary>
        /// 人工询价创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? querycreatedate { get; set; }
        /// <summary>
        /// 回价审批状态
        /// </summary>
        [SQLReadOnly]
        public int? approvalstatus { get; set; }
    }
}
