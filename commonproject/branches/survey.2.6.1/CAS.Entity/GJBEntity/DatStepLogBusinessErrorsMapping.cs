using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_StepLogBusinessErrorsMapping : DatStepLogBusinessErrorsMapping
    {
        /// <summary>
        /// 错误子类
        /// </summary>
        [SQLReadOnly]
        public string errordescript { get; set; }
        /// <summary>
        /// 错误父类
        /// </summary>
        [SQLReadOnly]
        public string typename { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        [SQLReadOnly]
        public string errorsremark { get; set; }

        /// <summary>
        /// 错误总数
        /// </summary>
        [SQLReadOnly]
        public int errorcount { get; set; }


    }
}
