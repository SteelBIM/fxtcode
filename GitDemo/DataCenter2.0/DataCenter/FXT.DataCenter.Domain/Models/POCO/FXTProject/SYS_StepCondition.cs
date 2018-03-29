using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_StepCondition
    {
        private int _conditionid;
        //[SQLField("conditionid", EnumDBFieldUsage.PrimaryKey, true)]
        public int conditionid
        {
            get { return _conditionid; }
            set { _conditionid = value; }
        }
        private string _entityclassname;
        /// <summary>
        /// 用于判断条件的参数所属的类
        /// </summary>
        public string entityclassname
        {
            get { return _entityclassname; }
            set { _entityclassname = value; }
        }
        private string _fieldname;
        /// <summary>
        /// 类的字段
        /// </summary>
        public string fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        private string _truevalue;
        /// <summary>
        /// 为真的值
        /// </summary>
        public string truevalue
        {
            get { return _truevalue; }
            set { _truevalue = value; }
        }
        private string _compareoperator;
        /// <summary>
        /// 比较操作符：>,<,>=,<=,=,!=,IN(包含)
        /// </summary>
        public string compareoperator
        {
            get { return _compareoperator; }
            set { _compareoperator = value; }
        }
        private int _conditiongroupid;
        /// <summary>
        /// 条件组
        /// </summary>
        public int conditiongroupid
        {
            get { return _conditiongroupid; }
            set { _conditiongroupid = value; }
        }

    }
}
