using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_NWorkWTLog")]
    public class DatNWorkWTLog : BaseTO
    {
        private int _consignerid;
        [SQLField("consignerid", EnumDBFieldUsage.PrimaryKey, true)]
        public int consignerid
        {
            get { return _consignerid; }
            set { _consignerid = value; }
        }
        private int _worktodoid;
        public int worktodoid
        {
            get { return _worktodoid; }
            set { _worktodoid = value; }
        }
        private int _userid;
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _remark;
        [MaxLength(500)]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }

}
