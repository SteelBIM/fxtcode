using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Object : DatObject
    {
        [SQLReadOnly]
        public string typecodename { get; set; }

        [SQLReadOnly]
        public string ownertypecodename { get; set; }

        [SQLReadOnly]
        public int provinceid { get; set; }

        /// <summary>
        /// 1-从询价添加,2-从委估对象添加的价格信息
        /// </summary>
        [SQLReadOnly]
        public int dataorigintype { get; set; }

        /// <summary>
        /// 询价Id
        /// </summary>
        [SQLReadOnly]
        public long qid { get; set; }
        [SQLReadOnly]
        public decimal unitprice { get; set; }
        [SQLReadOnly]
        public decimal totalprice { get; set; }
        [SQLReadOnly]
        public decimal tax { get; set; }
        [SQLReadOnly]
        public decimal netprice { get; set; }
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        [SQLReadOnly]
        public string action { get; set; }

        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public int surveystatecode { get; set; }
    }
}
