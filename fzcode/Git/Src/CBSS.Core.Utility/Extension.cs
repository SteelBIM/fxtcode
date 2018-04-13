using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Extension”的 XML 注释
   public static class Extension
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Extension”的 XML 注释
    {

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Extension.CutDoubleWithN(double, int)”的 XML 注释
        public static double CutDoubleWithN(this double d, int n)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Extension.CutDoubleWithN(double, int)”的 XML 注释
        {
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Double.Parse(strDecimal);
        }
    }
}
