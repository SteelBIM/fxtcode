using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class TreeView
    {
        public string Id { get; set; }
        public string text { get; set; }
        public string path { get; set; }
        public string productkeys { get; set; }
        public string ParentId { get; set; }
        public string tag { get; set; }
        public string schoolname { get; set; }
        private int _schoolid = 0;
        public int schoolid
        {
            get { return _schoolid; }
            set { _schoolid = value; }
        }

        public string createname { get; set; }

        public List<TreeView> nodes { get; set; }
        public CheckedCheck state { get; set; }

        private bool _iscontainnods = true;
        public bool isContainNods
        {
            get { return _iscontainnods; }
            set { _iscontainnods = value; }
        }

        public int Level { get; set; }
    }
    public class CheckedCheck
    {
        public bool showcheckbox { get; set; }
        public bool  @checked { get; set; }
        
    }
}