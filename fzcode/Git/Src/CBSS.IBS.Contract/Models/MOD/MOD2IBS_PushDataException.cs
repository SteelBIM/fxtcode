using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBSS.IBS.Contract
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
