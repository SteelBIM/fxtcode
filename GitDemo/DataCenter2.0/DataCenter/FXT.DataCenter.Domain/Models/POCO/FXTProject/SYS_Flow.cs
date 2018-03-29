using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Flow
    {
        private int _flowid;
        //[SQLField("flowid", EnumDBFieldUsage.PrimaryKey, true)]
        public int flowid
        {
            get { return _flowid; }
            set { _flowid = value; }
        }
        private int _flowtypeid;
        /// <summary>
        /// 流程类型
        /// </summary>
        public int flowtypeid
        {
            get { return _flowtypeid; }
            set { _flowtypeid = value; }
        }
        private string _flowname;
        public string flowname
        {
            get { return _flowname; }
            set { _flowname = value; }
        }
        private int _beginstepid;
        /// <summary>
        /// 流程开始的步骤Id
        /// </summary>
        public int beginstepid
        {
            get { return _beginstepid; }
            set { _beginstepid = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
