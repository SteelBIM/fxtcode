using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.MOD
{
    public class MOD2IBS_PushDataException
    {
        public int DataType;
        //类型处理
        public int ChangeType;

        public string Json;

        public string ErrorMessage;
    }
}
