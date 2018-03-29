using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_DisplayColumn_UserMapping")]
    public class DatDisplayColumnUserMapping : BaseTO
    {
        private int _mappingid;
        [SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
        public int mappingid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private int _displayid;
        public int displayid
        {
            get { return _displayid; }
            set { _displayid = value; }
        }
        private int _userid;
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int _sortid = 1;
        public int sortid
        {
            get { return _sortid; }
            set { _sortid = value; }
        }
    }
}