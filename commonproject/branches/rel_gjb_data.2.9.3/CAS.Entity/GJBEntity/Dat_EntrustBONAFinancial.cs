using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_EntrustBONAFinancial : DatEntrustBONAFinancial
    {
        /// <summary>
        /// 业务员 ID
        /// </summary>
        [SQLReadOnly]
        public int? businessuserid { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        [SQLReadOnly]
        public string businessusername { get; set; }
        /// <summary>
        /// 委托方
        /// </summary>
        [SQLReadOnly]
        public string clientname { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        [SQLReadOnly]
        public string datasource { get; set; }
        /// <summary>
        /// 业务员机构
        /// </summary>
        [SQLReadOnly]
        public string bumenname { get; set; }
        /// <summary>
        /// 委托时间
        /// </summary>
        [SQLReadOnly]
        public DateTime entrustdate { get; set; }
        /// <summary>
        /// 业务员登录用户名
        /// </summary>
        public string businessuserloinname { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int bumenid { get; set; }
        /// <summary>
        /// 按揭类型名称
        /// </summary>
        public string mortgagetypename { get; set; }
        /// <summary>
        /// 业务创建用户中文名称 Alex 2016-03-17
        /// </summary>
        public string createusername { get; set; }
        /// <summary>
        /// 业务创建用户中文名 Alex 2016-03-17
        /// </summary>
        public string username { get; set; }
    }
}
