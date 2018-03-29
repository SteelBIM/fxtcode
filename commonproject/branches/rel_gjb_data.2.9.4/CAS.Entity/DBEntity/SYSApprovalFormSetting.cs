using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_ApprovalFormSetting")]
    public class SYSApprovalFormSetting : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _reportstage;
        public int reportstage
        {
            get { return _reportstage; }
            set { _reportstage = value; }
        }
        private string _biztype;
        public string biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private string _biztypename;
        public string biztypename
        {
            get { return _biztypename; }
            set { _biztypename = value; }
        }
        private string _reporttypecode;
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttypename;
        public string reporttypename
        {
            get { return _reporttypename; }
            set { _reporttypename = value; }
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
        private int? _createuserid;
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _createdate;
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool? _valid;
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }


}
