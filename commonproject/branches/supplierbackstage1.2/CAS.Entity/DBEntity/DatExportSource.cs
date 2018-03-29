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
    }
}