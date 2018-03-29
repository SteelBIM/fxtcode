using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_FlowLog
    {
        private long _flowlogid;
        //[SQLField("flowlogid", EnumDBFieldUsage.PrimaryKey, true)]
        public long flowlogid
        {
            get { return _flowlogid; }
            set { _flowlogid = value; }
        }
        private int _flowid;
        public int flowid
        {
            get { return _flowid; }
            set { _flowid = value; }
        }
        private string _createuser;
        /// <summary>
        /// 流程的创建/申请人
        /// </summary>
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

    }
}
