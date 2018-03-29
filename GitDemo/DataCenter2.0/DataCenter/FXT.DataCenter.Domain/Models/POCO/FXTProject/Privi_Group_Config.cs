using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Group_Config
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _priviname;
        /// <summary>
        /// 权限名称
        /// </summary>
        public string priviname
        {
            get { return _priviname; }
            set { _priviname = value; }
        }
        private int _configid;
        /// <summary>
        /// 权限组合ID
        /// </summary>
        //[SQLField("configid", EnumDBFieldUsage.PrimaryKey)]
        public int configid
        {
            get { return _configid; }
            set { _configid = value; }
        }
        private int _groupid;
        /// <summary>
        /// 组ID
        /// </summary>
        //[SQLField("groupid", EnumDBFieldUsage.PrimaryKey)]
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private int? _valid;
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _savedate;
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _createuser;
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
