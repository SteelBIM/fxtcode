using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{

    public class Dat_ChargeInOutRecord : DatChargeInOutRecord
    {
        /// <summary>
        /// 收入支出类型名称
        /// </summary>
        public string inouttypename { get; set; }
        /// <summary>
        /// 收费形式名称
        /// </summary>
        public string chargetypename { get; set; }
        /// <summary>
        /// 确认未确认类型
        /// </summary>
        public string isverifyname { get; set; }

        /// <summary>
        ///记录人真是姓名
        /// </summary>
        public string recordpersonname { get; set; }

        /// <summary>
        /// 确认人真实姓名
        /// </summary>
        public string verifypersontruename { get; set; }

        /// <summary>
        /// 经手人真实姓名
        /// </summary>
        public string handpersonname { get; set; }
        /// <summary>
        /// 缴费人/委托人
        /// </summary>
        public string clientname { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string businessusername { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string biztypename { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypecodename { get; set; }
        /// <summary>
        /// 分支机构
        /// </summary>
        public string subcompanyname { get; set; }
        /// <summary>
        /// 客户结构全称
        /// </summary>
        public string customercompanyfullname { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno { get; set; }
        /// <summary>
        /// 业务表ID
        /// </summary>
        public long? eid { get; set; }
        /// <summary>
        /// 支出类型名称
        /// </summary>
        public string outtypename { get; set; }

    }
}
