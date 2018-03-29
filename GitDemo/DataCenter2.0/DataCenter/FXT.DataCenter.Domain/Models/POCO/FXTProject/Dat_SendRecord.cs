using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_SendRecord
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _sender;
        public string sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        private string _accepter;
        public string accepter
        {
            get { return _accepter; }
            set { _accepter = value; }
        }
        private int? _type;
        public int? type
        {
            get { return _type; }
            set { _type = value; }
        }
        private DateTime? _createtime;
        public DateTime? createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int? _surveyid;
        public int? surveyid
        {
            get { return _surveyid; }
            set { _surveyid = value; }
        }
        private string _code;
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

    }
}
