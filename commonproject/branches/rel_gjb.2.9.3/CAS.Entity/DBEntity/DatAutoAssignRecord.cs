using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_AutoAssignRecord")]
    public class DatAutoAssignRecord : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _settingid;
        /// <summary>
        /// 自动分配规则主键
        /// </summary>
        public int settingid
        {
            get { return _settingid; }
            set { _settingid = value; }
        }
        private int _assigninperson;
        /// <summary>
        /// 分配给谁
        /// </summary>
        public int assigninperson
        {
            get { return _assigninperson; }
            set { _assigninperson = value; }
        }
        private DateTime _assigndate;
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime assigndate
        {
            get { return _assigndate; }
            set { _assigndate = value; }
        }

        private int _ruletype;
        /// <summary>
        /// 规则类型 1、业务量最少 2、轮循 3、随机
        /// </summary>
        public int ruletype
        {
            get { return _ruletype; }
            set { _ruletype = value; }
        }
        [SQLReadOnly]
        public int assigncount { get; set; }
    }
}