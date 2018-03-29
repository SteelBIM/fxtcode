using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Step
    {
        private int _stepid;
        //[SQLField("stepid", EnumDBFieldUsage.PrimaryKey, true)]
        public int stepid
        {
            get { return _stepid; }
            set { _stepid = value; }
        }
        private string _stepname;
        /// <summary>
        /// 流程步骤名称
        /// </summary>
        public string stepname
        {
            get { return _stepname; }
            set { _stepname = value; }
        }
        private int _flowid;
        /// <summary>
        /// 对应的流程id
        /// </summary>
        public int flowid
        {
            get { return _flowid; }
            set { _flowid = value; }
        }
        private int? _customformid;
        /// <summary>
        /// 此步骤客户自定义表单
        /// </summary>
        public int? customformid
        {
            get { return _customformid; }
            set { _customformid = value; }
        }
        private byte _isend = ((0));
        /// <summary>
        /// 是否结束步骤，必须有一个结束步骤
        /// </summary>
        public byte isend
        {
            get { return _isend; }
            set { _isend = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
