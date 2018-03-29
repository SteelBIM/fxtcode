using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Customer_Function
    {
        private long _id;
        /// <summary>
        /// CAS功能权限设置（只存关闭的功能）
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
        private int _companyid = 0;
        /// <summary>
        /// 客户ID(0表示评估机构本身)
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _functioncode;
        /// <summary>
        /// 功能（1032）
        /// </summary>
        public int functioncode
        {
            get { return _functioncode; }
            set { _functioncode = value; }
        }
        private int _isclose = 1;
        /// <summary>
        /// 是否关闭
        /// </summary>
        public int isclose
        {
            get { return _isclose; }
            set { _isclose = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

    }
}
