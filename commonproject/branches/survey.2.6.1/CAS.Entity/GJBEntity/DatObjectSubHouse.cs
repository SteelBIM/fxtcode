using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ObjectSubHouse : DatObjectSubHouse
    {
        /// <summary>
        /// 附属房屋类型名称
        /// </summary>
        [SQLReadOnly]
        public string subhousetypename { get; set; }

        [SQLReadOnly]
        public string action { get; set; }

        /// <summary>
        /// 附属房屋总价
        /// </summary>
        [SQLReadOnly]
        public decimal? sumtotalprice { get; set; }

        /// <summary>
        /// 附属房屋信息操作
        /// </summary>
        [SQLReadOnly]
        public string subhouseacton { get; set; }
    }
}
