using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class SyncRangmodel
    {
        public tb_batchactivateuse ActivateUse { get; set; }
        public tb_batchactivateusedevice ActivateUseDevice { get; set; }
    }
    public class SyncRangmodel_copy
    {
        public tb_batchactivateuse_copy ActivateUse { get; set; }
        public tb_batchactivateusedevice_copy ActivateUseDevice { get; set; }
    }
}
