using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_SecondModule
    {
        public string SecondModuleID { get; set; }
        public string SecondModuleName { get; set; }
        public string EnglishName { get; set; }
        public int? RequiredTimes { get; set; }
        public int FinishedTimes { get; set; }
        public List<v_SubModuleDetails> History { get; set; }
    }
    public class v_SubModuleDetails
    {
        public string RoleName { get; set; }
        public DateTime? Date { get; set; }
        public double? AverageScore { get; set; }
    }
}
