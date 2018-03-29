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


    }
}
