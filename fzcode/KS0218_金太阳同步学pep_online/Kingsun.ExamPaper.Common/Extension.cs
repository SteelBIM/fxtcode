using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.ExamPaper.Common
{
    public static class Extension
    {
        public static decimal CutDoubleWithN(this decimal d, int n)
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
            return Decimal.Parse(strDecimal);
        }  
    }
}
