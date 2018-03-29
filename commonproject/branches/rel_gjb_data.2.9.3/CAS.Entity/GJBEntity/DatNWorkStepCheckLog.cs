using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkStepCheckLog : DatNWorkStepCheckLog
    {
        [SQLReadOnly]
        public string truename { get; set; }
        [SQLReadOnly]
        public string flownodename { get; set; }
        /// <summary>
        /// 办理人Id
        /// </summary>
        [SQLReadOnly]
        public int checkid { get; set; }

        /// <summary>
        /// 登记环节
        /// </summary>
        [SQLReadOnly]
        public string nodename { get; set; }

        [SQLReadOnly]
        public string consignertruename { get; set; }
        List<Dat_StepLogBusinessErrorsMapping> _errors = new List<Dat_StepLogBusinessErrorsMapping>();
        [SQLReadOnly]
        public List<Dat_StepLogBusinessErrorsMapping> errors 
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }
        /// <summary>
        /// 电子签名
        /// </summary>
        [SQLReadOnly]
        public string electronicsignature { get; set; }
    }
}
