using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkToDo : DatNWorkToDo
    {
        [SQLReadOnly]
        public string formname { get; set; }
        [SQLReadOnly]
        public string workflowname { get; set; }
        [SQLReadOnly]
        public string nodename { get; set; }
        [SQLReadOnly]
        public string fullname { get; set; }
        [SQLReadOnly]
        public string shenpitruenamelist { get; set; }
        [SQLReadOnly]
        public string consignertruenamelist { get; set; }
        [SQLReadOnly]
        public string oktruenamelist { get; set; }
        [SQLReadOnly]
        public string readertruenamelist { get; set; }
        [SQLReadOnly]
        public int userid { get; set; }
        [SQLReadOnly]
        public string truename { get; set; }
        private List<Dat_NWorkStepLog> _steploglist = new List<Dat_NWorkStepLog>();
        [SQLReadOnly]
        public List<Dat_NWorkStepLog> steploglist
        {
            get
            {
                return _steploglist;
            }
            set
            {
                _steploglist = value;
            }
        }
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public string pstype { get; set; }
        [SQLReadOnly]
        public string sptype { get; set; }
        [SQLReadOnly]
        public string reportno { get; set; }
        [SQLReadOnly]
        public string ypnumber { get; set; }
        [SQLReadOnly]
        public string ifcanedit { get; set; }
        [SQLReadOnly]
        public int? workflowtype { get; set; }
        [SQLReadOnly]
        public string wtremark { get; set; }
        /// <summary>
        /// 流程已操作的步骤是否包含已盖章
        /// </summary>
        [SQLReadOnly]
        public bool containsstampnode { get; set; }
    }
}
