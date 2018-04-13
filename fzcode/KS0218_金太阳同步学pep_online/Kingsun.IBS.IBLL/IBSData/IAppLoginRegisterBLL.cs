using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL
{
    public interface IAppLoginRegisterBLL
    {
        void IBSRegisterUser2Mod();
        void IBSRegisterUser2ModReTryFirstTime();
        void IBSRegisterUser2ModReTrySecondTime();
    }
}
