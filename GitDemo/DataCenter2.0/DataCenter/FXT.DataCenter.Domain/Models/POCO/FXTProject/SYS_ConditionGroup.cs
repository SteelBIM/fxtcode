using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_ConditionGroup
    {
        private int _conditiongroupid;
        //[SQLField("conditiongroupid", EnumDBFieldUsage.PrimaryKey, true)]
        public int conditiongroupid
        {
            get { return _conditiongroupid; }
            set { _conditiongroupid = value; }
        }
        private int _stepid;
        /// <summary>
        /// 条件组所属的步骤
        /// </summary>
        public int stepid
        {
            get { return _stepid; }
            set { _stepid = value; }
        }
        private byte _stepstatus;
        /// <summary>
        /// 审核通过1，不通过2， 3暂缓
        /// </summary>
        public byte stepstatus
        {
            get { return _stepstatus; }
            set { _stepstatus = value; }
        }
        private string _conditionrelation;
        /// <summary>
        /// 各条件的关系表达式，可实现复杂的与或非关系。
        /// </summary>
        public string conditionrelation
        {
            get { return _conditionrelation; }
            set { _conditionrelation = value; }
        }
        private int? _nextstep;
        /// <summary>
        /// 根据审核状态流程流转的下一个步骤
        /// </summary>
        public int? nextstep
        {
            get { return _nextstep; }
            set { _nextstep = value; }
        }

    }
}
