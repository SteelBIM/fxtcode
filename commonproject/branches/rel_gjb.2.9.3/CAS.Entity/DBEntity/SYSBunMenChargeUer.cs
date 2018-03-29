using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_BunMenChargeUer")]
    public class SYSBunMenChargeUer : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _bumenid;
        /// <summary>
        /// 部门
        /// </summary>
        public int bumenid
        {
            get { return _bumenid; }
            set { _bumenid = value; }
        }
        private int _chargeuserid;
        /// <summary>
        /// 部门负责人
        /// </summary>
        public int chargeuserid
        {
            get { return _chargeuserid; }
            set { _chargeuserid = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }
    }
}
