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
        /// 业务类型code
        /// </summary>
        [SQLReadOnly]
        public int biztype { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypecodename { get; set; }
        /// <summary>
        /// 报告类型code
        /// </summary>
        [SQLReadOnly]
        public int reporttypecode { get; set; }
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
        /// <summary>
        /// 统计数据
        /// </summary>
        public ChargeInOutRecordSum chargeinoutrecordsum
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型 预评、报告
        /// </summary>
        [SQLReadOnly]
        public int reportstage { get; set; }

        /// <summary>
        /// 预评编号
        /// </summary>
        [SQLReadOnly]
        public string ypnumber { get; set; }

    }


    [Serializable]
    /// <summary>
    /// 用于各项需要统计的收支总额
    /// </summary>    
    public class ChargeInOutRecordSum : BaseTO
    {
        /// <summary>
        /// 总支出
        /// </summary>
        public decimal zhichu { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal shouru { get; set; }
        /// <summary>
        /// 总提成
        /// </summary>
        public decimal ticheng { get; set; }
        /// <summary>
        /// 总返利
        /// </summary>
        public decimal fanli { get; set; }
        /// <summary>
        /// 总应收
        /// </summary>
        public decimal yingshou { get; set; }
        /// <summary>
        /// 总未收
        /// </summary>
        public decimal weishou { get; set; }
        /// <summary>
        /// 总开票
        /// </summary>
        public decimal kaipiao { get; set; }
        /// <summary>
        /// 总退费
        /// </summary>
        public decimal tuifei { get; set; }

    }
}
