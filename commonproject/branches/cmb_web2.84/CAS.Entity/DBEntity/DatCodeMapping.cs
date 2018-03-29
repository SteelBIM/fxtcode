using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CodeMapping")]
    public class DatCodeMapping : BaseTO
    {
        private int _code;
        [SQLField("code", EnumDBFieldUsage.PrimaryKey)]
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }
        private int? _adjustamount;
        public int? adjustamount
        {
            get { return _adjustamount; }
            set { _adjustamount = value; }
        }
    }
}