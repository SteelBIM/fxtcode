using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Common
{
    /// <summary>
    /// 中文一二和数字123进行合并比较大小,作为不同的判断标准
    /// 其他规则还是根据字符大小判断
    /// </summary>
    public class CNComparer : IComparer<string>
    {
        public readonly static Dictionary<char, int> cnStrArray = new Dictionary<char, int>() { 
            {'零', 0},
            {'一', 1},
            {'二', 2},
            {'三', 3},
            {'四', 4},
            {'五', 5},
            {'六', 6},
            {'七', 7},
            {'八', 8},
            {'九', 9},
            {'十', 10},
            {'千', 11},
            {'百', 12},
            {'万', 13},
            {'亿', 14},
            {'甲', 15},
            {'乙', 16},
            {'丙', 17},
            {'丁', 18},
            {'戊', 19},
            {'己', 20},
            {'庚', 21},
            {'辛', 22},
            {'壬', 23},
            {'癸', 24},
            {'东', 25},
            {'西', 26},
            {'南', 27},
            {'北', 28},
            {'中', 29}
        };

        public readonly static Dictionary<char, int> cnNumStrArray = new Dictionary<char, int>() { 
            {'零', 0},
            {'一', 1},
            {'二', 2},
            {'三', 3},
            {'四', 4},
            {'五', 5},
            {'六', 6},
            {'七', 7},
            {'八', 8},
            {'九', 9},
            {'十', 10},
            {'千', 11},
            {'百', 12},
            {'万', 13},
            {'亿', 14}
        };

        public char ToSBC(char c)
        {
            if (c > 65280 && c < 65375)//其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
                return (char)(c - 65248);
            return c;
        }

        public int Compare(string x, string y)
        {
            if (x == null && y != null)
            {
                return -1;
            }
            if (x != null && y == null)
            {
                return 1;
            }
            if (x == null && y == null)
            {
                return 0;
            }
            string fileA = x;
            string fileB = y;
            char[] arr1 = fileA.ToCharArray();
            char[] arr2 = fileB.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += ToSBC(arr1[i]);
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += ToSBC(arr2[j]);
                        j++;
                    }
                    if (int.Parse(s1) > int.Parse(s2))
                    {
                        return 1;
                    }
                    if (int.Parse(s1) < int.Parse(s2))
                    {
                        return -1;
                    }
                }
                else if(cnNumStrArray.Keys.Contains(arr1[i])&&cnNumStrArray.Keys.Contains(arr2[i]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && cnNumStrArray.Keys.Contains(arr1[i]))
                    {
                        s1 += cnNumStrArray[arr1[i]];
                        i++;
                    }
                    while (j < arr2.Length && cnNumStrArray.Keys.Contains(arr2[j]))
                    {
                        s2 += cnNumStrArray[arr2[j]];
                        j++;
                    }
                    if (int.Parse(s1) > int.Parse(s2))
                    {
                        return 1;
                    }
                    if (int.Parse(s1) < int.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    int a1;
                    int a2;
                    if (cnStrArray.Keys.Contains(arr1[i]) && cnStrArray.Keys.Contains(arr2[j]))
                    {
                        a1 = cnStrArray[arr1[i]];
                        a2 = cnStrArray[arr2[j]];
                    }
                    else
                    {
                        a1 = arr1[i];
                        a2 = arr2[j];
                    }
                    if (a1 > a2)
                    {
                        return 1;
                    }
                    if (a1 < a2)
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
        }
    }
}
