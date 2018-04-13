using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class TreeModel
    {
        public string name { get; set; }
        public string checkboxValue { get; set; }
        public bool Checked { get; set; }
        public object children { get; set; }
        

    }
}
