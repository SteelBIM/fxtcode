using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_DiscountPrice
    {
        private long _id;
        /// <summary>
        /// 客户自动估价折扣价系数
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 客户单位ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private decimal _discount = 1M;
        /// <summary>
        /// 折扣系数(例如:0.95)
        /// </summary>
        public decimal discount
        {
            get { return _discount; }
            set { _discount = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _savedate;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuserid;
        /// <summary>
        /// 最好修改人
        /// </summary>
        public string saveuserid
        {
            get { return _saveuserid; }
            set { _saveuserid = value; }
        }

    }
}
