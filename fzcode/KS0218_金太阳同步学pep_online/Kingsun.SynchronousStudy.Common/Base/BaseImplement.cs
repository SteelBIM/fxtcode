using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsun.SynchronousStudy.Common.Base
{
    public abstract class BaseImplement
    {
        public abstract KingResponse ProcessRequest(KingRequest request);
    }
}
