using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ExportSource")]
    public class DatExportSource : BaseTO
    {
        private int _btfsid;
        public int btfsid
        {
            get { return _btfsid; }
            set { _btfsid = value; }
        }
        private string _querycolumn;
        /// <summary>
        /// 该列在数据导出场景下的查询SQL语句
        /// </summary>
        public string querycolumn
        {
            get { return _querycolumn; }
            set { _querycolumn = value; }
        }        
        private int? _typecode;
        /// <summary>
        /// 列表数据类型（报告、预评等）
        /// </summary>
        public int? typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int? _subtypecode;
        /// <summary>
        /// 列表数据子类型（如委估对象中的住宅、商业等）
        /// </summary>
        public int? subtypecode
        {
            get { return _subtypecode; }
            set { _subtypecode = value; }
        }
        
        private int _columndatatype;
        /// <summary>
        /// 字段类型 （0：其他）
        /// </summary>
        public int columndatatype
        {
            get { return _columndatatype; }
            set { _columndatatype = value; }
        }
    }
}