using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FxtCenterTest
{
    public class ParseClass
    {
        /// <summary>  
        /// 转换数字  
        /// </summary>  
        protected static long CharToNumber(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '零': return 0;
                default: return -1;
            }
        }

        /// <summary>  
        /// 转换单位  
        /// </summary>  
        protected static long CharToUnit(char c)
        {
            switch (c)
            {
                case '十': return 10;
                case '百': return 100;
                case '千': return 1000;
                case '万': return 10000;
                case '亿': return 100000000;
                default: return -1;
            }
        }
        /// <summary>  
        /// 将中文数字转换阿拉伯数字  
        /// </summary>  
        /// <param name="cnum">汉字数字</param>  
        /// <returns>长整型阿拉伯数字</returns>  
        public static long ParseCnToInt(string cnum)
        {
            cnum = Regex.Replace(cnum, "\\s+", "");
            long firstUnit = 1;//一级单位                  
            long secondUnit = 1;//二级单位   
            long tmpUnit = 1;//临时单位变量  
            long result = 0;//结果  
            for (int i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理  
            {
                tmpUnit = CharToUnit(cnum[i]);//取出此位对应的单位  
                if (tmpUnit==-1)
                {
                    
                }
                if (tmpUnit > firstUnit)//判断此位是数字还是单位  
                {
                    firstUnit = tmpUnit;//是的话就赋值,以备下次循环使用  
                    secondUnit = 1;
                    if (i == 0)//处理如果是"十","十一"这样的开头的  
                    {
                        result += firstUnit * secondUnit;
                    }
                    continue;//结束本次循环  
                }
                else if (tmpUnit > secondUnit)
                {
                    secondUnit = tmpUnit;
                    continue;
                }
                result += firstUnit * secondUnit * CharToNumber(cnum[i]);//如果是数字,则和单位想乘然后存到结果里  
            }
            return result;
        } 
    }
}