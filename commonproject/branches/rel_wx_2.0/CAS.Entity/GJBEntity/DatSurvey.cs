using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Survey:DatSurvey
    {
        /// <summary>
        /// 查勘对象名
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }
        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public string statecodename { get; set; }

        /// <summary>
        /// 查勘对象类型
        /// </summary>
        [SQLReadOnly]
        public string typecodename { get; set; }

        /// <summary>
        /// 查勘员
        /// </summary>
        [SQLReadOnly]
        public string username { get; set; }

        /// <summary>
        /// 分支机构名称
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }

        /// <summary>
        /// 分配人
        /// </summary>
        [SQLReadOnly]
        public string assignusername { get; set; }

        /// <summary>
        /// 查勘等级
        /// </summary>
        [SQLReadOnly]
        public string surveyclassname { get; set; }
    }
}
