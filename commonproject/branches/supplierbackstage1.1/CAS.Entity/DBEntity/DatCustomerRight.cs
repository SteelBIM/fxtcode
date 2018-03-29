using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CustomerRight")]
    public class DatCustomerRight : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _productcode;
        /// <summary>
        /// 产品Code
        /// </summary>
        public int productcode
        {
            get { return _productcode; }
            set { _productcode = value; }
        }
        private int _rightno;
        /// <summary>
        /// 功能编号
        /// </summary>
        public int rightno
        {
            get { return _rightno; }
            set { _rightno = value; }
        }
        private string _rights;
        /// <summary>
        /// 功能权限
        /// </summary>
        public string rights
        {
            get { return _rights; }
            set { _rights = value; }
        }
    }
}