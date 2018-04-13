using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TreeView
    {
        public string Id { get; set; }
        public string text { get; set; }
        public string ParentId { get; set; }
        public string tag { get; set; }
        public bool isContainNods { get; set; }
        public List<TreeView> nodes { get; set; } 
    }
}
