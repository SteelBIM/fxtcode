using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CustomerBelongsGroup")]
    public class DatCustomerBelongsGroup : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _customerid;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }
        private DateTime _overdate;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private DateTime _createdate=DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _groupid;
        /// <summary>
        /// 客户组ID
        /// </summary>
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}