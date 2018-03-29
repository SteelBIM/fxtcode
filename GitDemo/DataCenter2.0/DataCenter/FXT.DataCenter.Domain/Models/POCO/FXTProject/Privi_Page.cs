using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Page
    {
        private int _uiid;
        //[SQLField("uiid", EnumDBFieldUsage.PrimaryKey, true)]
        public int uiid
        {
            get { return _uiid; }
            set { _uiid = value; }
        }
        private string _pageid;
        public string pageid
        {
            get { return _pageid; }
            set { _pageid = value; }
        }
        private string _buttonid;
        public string buttonid
        {
            get { return _buttonid; }
            set { _buttonid = value; }
        }
        private int _syscode;
        public int syscode
        {
            get { return _syscode; }
            set { _syscode = value; }
        }
        private string _uiname;
        public string uiname
        {
            get { return _uiname; }
            set { _uiname = value; }
        }
        private int? _functioncode;
        public int? functioncode
        {
            get { return _functioncode; }
            set { _functioncode = value; }
        }
    }

}
