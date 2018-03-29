using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ExportFieldExt")]
    public class DatExportFieldExt : BaseTO
    {
        private int _btfsid;
        [SQLField("btfsid", EnumDBFieldUsage.PrimaryKey)]
        public int btfsid
        {
            get { return _btfsid; }
            set { _btfsid = value; }
        }
        private string _fieldname;
        public string fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        private string _showname;
        public string showname
        {
            get { return _showname; }
            set { _showname = value; }
        }
    }
}