using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ExportColumnMapping")]
    public class DatExportColumnMapping : BaseTO
    {
        private int _ecid;
        [SQLField("ecid", EnumDBFieldUsage.PrimaryKey, true)]
        public int ecid
        {
            get { return _ecid; }
            set { _ecid = value; }
        }
        private int _btfsid;
        public int btfsid
        {
            get { return _btfsid; }
            set { _btfsid = value; }
        }
        private int _sortid;
        /// <summary>
        /// 排序
        /// </summary>
        public int sortid
        {
            get { return _sortid; }
            set { _sortid = value; }
        }
        private int _etid;
        public int etid
        {
            get { return _etid; }
            set { _etid = value; }
        }
    }
}