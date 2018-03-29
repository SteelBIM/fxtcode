using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_FlowType
    {
        private int _flowtypeid;
        //[SQLField("flowtypeid", EnumDBFieldUsage.PrimaryKey, true)]
        public int flowtypeid
        {
            get { return _flowtypeid; }
            set { _flowtypeid = value; }
        }
        private string _flowtypename;
        public string flowtypename
        {
            get { return _flowtypename; }
            set { _flowtypename = value; }
        }
        private int? _parenttypeid;
        /// <summary>
        /// 流程类型的父类型
        /// </summary>
        public int? parenttypeid
        {
            get { return _parenttypeid; }
            set { _parenttypeid = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
