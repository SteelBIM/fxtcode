using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_VideoDetails
    {
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string BookName { get; set; }
        public int FirstTitleID { get; set; }
        public string FirstTitle { get; set; }
        public int SecondTitleID { get; set; }
        public string SecondTitle { get; set; }
        public string VideoTitle { get; set; }
        public int FirstModularID { get; set; }
        public int SecondModularID { get; set; }
        public string ModularName { get; set; }
    }
}
