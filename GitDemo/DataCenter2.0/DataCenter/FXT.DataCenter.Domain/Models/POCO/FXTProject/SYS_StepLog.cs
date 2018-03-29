using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_StepLog
    {
        private long _steplogid;
        /// <summary>
        /// 记录步骤的信息
        /// </summary>
        //[SQLField("steplogid", EnumDBFieldUsage.PrimaryKey, true)]
        public long steplogid
        {
            get { return _steplogid; }
            set { _steplogid = value; }
        }
        private int _stepid;
        public int stepid
        {
            get { return _stepid; }
            set { _stepid = value; }
        }
        private long _flowlogid;
        /// <summary>
        /// 流程记录Id
        /// </summary>
        public long flowlogid
        {
            get { return _flowlogid; }
            set { _flowlogid = value; }
        }
        private string _checkuserid;
        /// <summary>
        /// 审核用户Id
        /// </summary>
        public string checkuserid
        {
            get { return _checkuserid; }
            set { _checkuserid = value; }
        }
        private DateTime _checkdate = DateTime.Now;
        public DateTime checkdate
        {
            get { return _checkdate; }
            set { _checkdate = value; }
        }
        private byte _stepstatus;
        /// <summary>
        /// 审核状态-1-不通过，1-通过，0-暂缓
        /// </summary>
        public byte stepstatus
        {
            get { return _stepstatus; }
            set { _stepstatus = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
