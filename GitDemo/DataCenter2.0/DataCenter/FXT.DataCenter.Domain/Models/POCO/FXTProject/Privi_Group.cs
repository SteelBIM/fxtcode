using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Group
    {
        private int _groupid;
        //[SQLField("groupid", EnumDBFieldUsage.PrimaryKey, true)]
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private string _groupname;
        public string groupname
        {
            get { return _groupname; }
            set { _groupname = value; }
        }
        private int? _grouptypecode;
        /// <summary>
        /// 客户类型（1.0版云估价）
        /// </summary>
        public int? grouptypecode
        {
            get { return _grouptypecode; }
            set { _grouptypecode = value; }
        }
        private int _companyid = 0;
        /// <summary>
        /// 所属公司ID，0表示预设组
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private byte? _grouplevel;
        /// <summary>
        /// 1-预设组，2-用户定义组
        /// </summary>
        public byte? grouplevel
        {
            get { return _grouplevel; }
            set { _grouplevel = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
