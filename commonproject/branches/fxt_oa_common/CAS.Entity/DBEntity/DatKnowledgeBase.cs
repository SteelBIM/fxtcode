using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_KnowledgeBase")]
    public class DatKnowledgeBase : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _subject;
        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        private string _content;
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private long _createuserid;
        public long createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _lastupdatedate;
        public DateTime lastupdatedate
        {
            get { return _lastupdatedate; }
            set { _lastupdatedate = value; }
        }
        private long _lastupdateuserid;
        public long lastupdateuserid
        {
            get { return _lastupdateuserid; }
            set { _lastupdateuserid = value; }
        }

        public bool valid { get; set; }

        public DateTime createdate { get; set; }
    }
}