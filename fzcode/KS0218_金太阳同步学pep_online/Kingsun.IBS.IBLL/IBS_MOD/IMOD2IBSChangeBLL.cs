using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL.IBS_MOD
{
    public interface IMOD2IBSChangeBLL
    {
        void Change();
        void CreateProcTable();

        void MOD2IBSFirstCreateData();
        void MOD2IBSUserInfo();
        void MOD2IBSClassInfo();
        void MOD2IBSAreaInfo();
        void MOD2IBSSchInfo();

        void IBSRepairTrueName();

        void IBSRepairClassUserTrueName();
    }
}
