using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_ListSubModules
    {
        public string SecondModularID { get; set; }
        public string ModularName { get; set; }
        public string ModularEN { get; set; }
        public string RepeatNumber { get; set; }
        public string SerialNumber { get; set; }
        public string TextSerialNumber { get; set; }

        public int SecondTitleID { get; set; }
    }
    public class v_ListSubModules_0
    {
        public int SecondModularID { get; set; }
        public string ModularName { get; set; }
        public string ModularEN { get; set; }
        public int RepeatNumber { get; set; }
        public int SerialNumber { get; set; }
        public int TextSerialNumber { get; set; }

        public int? SecondTitleID { get; set; }

        public string Content { get; set; }
    }
    public class ListSubModuleCompare : IEqualityComparer<v_ListSubModules>
    {
        // 摘要: 
        //     确定指定的对象是否相等。
        //
        // 参数: 
        //   x:
        //     要比较的第一个对象。
        //
        //   y:
        //     要比较的第二个对象。
        //
        // 返回结果: 
        //     如果指定的对象相等，则为 true；否则为 false。
        //
        // 异常: 
        //   System.ArgumentException:
        //     x 和 y 的类型不同，它们都无法处理与另一个进行的比较。
        public bool Equals(v_ListSubModules x, v_ListSubModules y)
        {
            return (x.ModularEN == y.ModularEN) && (x.ModularName == y.ModularName) && (x.RepeatNumber == y.RepeatNumber) && (x.SecondModularID == y.SecondModularID) && (x.SerialNumber == y.SerialNumber) && x.TextSerialNumber == y.TextSerialNumber;
        }
        //
        // 摘要: 
        //     返回指定对象的哈希代码。
        //
        // 参数: 
        //   obj:
        //     System.Object，将为其返回哈希代码。
        //
        // 返回结果: 
        //     指定对象的哈希代码。
        //
        // 异常: 
        //   System.ArgumentNullException:
        //     obj 的类型为引用类型，obj 为 null。
        public int GetHashCode(v_ListSubModules obj)
        {
            return 0;
        }
    }

}
