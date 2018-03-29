using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ExportColumnMapping : DatExportColumnMapping
    {
        [SQLReadOnly]
        public string fieldname { get; set; }
        [SQLReadOnly]
        public string showname { get; set; }
        [SQLReadOnly]
        public string querycolumn { get; set; }
        /// <summary>
        /// 字段类型 （0：其他）
        /// </summary>
        [SQLReadOnly]
        public int columndatatype { get; set; }
        [SQLReadOnly]
        public bool issystemfield { get; set; }
        /// <summary>
        /// 字段对应的表单数据类型
        /// </summary>
        [SQLReadOnly]
        public string tablefieldtypecodename { get; set; }
    }
}
