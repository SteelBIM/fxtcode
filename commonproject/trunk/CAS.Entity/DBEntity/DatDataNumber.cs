using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_DataNumber")]
    public class DatDataNumber : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.None,true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _objectnumber;
        /// <summary>
        /// 编号
        /// </summary>
        [SQLField("objectnumber", EnumDBFieldUsage.PrimaryKey)]
        public long objectnumber
        {
            get { return _objectnumber; }
            set { _objectnumber = value; }
        }
        private int _objecttypecode;
        /// <summary>
        /// 数据类型：2018010询价编号，2018001业务编号
        /// </summary>
        [SQLField("objecttypecode", EnumDBFieldUsage.PrimaryKey)]
        public int objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
    }

}