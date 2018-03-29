using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterServiceOpen.Actualize.Common
{
    [Serializable]
    public class JsonReturnData
    {
        public int returntype { get; set; } //1为正确
        public object returntext { get; set; }
        public object data { get; set; }
        public object debug { get; set; }
    }
}
