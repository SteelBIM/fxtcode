using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CAS.Common
{
    public class CodeHelper
    {
        /// <summary>
        /// 获取计算税费所需条件的codes
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int[] TaxesTemplateCode(string condition)
        {
            int[] str = new int[5];
            switch (condition[0])
            {
                case '0':
                    str[0] = 4002001; break;
                case '1':
                    str[0] = 4002002; break;
                default:
                    str[0] = 4002003; break;
            }
            switch (condition[1])
            {
                case '0':
                    str[1] = 4003001; break;
                case '1':
                    str[1] = 4003002; break;
                default:
                    str[1] = 4003003; break;
            }
            switch (condition[2])
            {
                case '0':
                    str[2] = 4004001; break;
                case '1':
                    str[2] = 4004002; break;
                default:
                    str[2] = 4004003; break;
            }
            switch (condition[3])
            {
                case '0':
                    str[3] = 4005001; break;
                case '1':
                    str[3] = 4005002; break;
                default:
                    str[3] = 4005003; break;
            }
            switch (condition[4])
            {
                case '0':
                    str[4] = 4006001; break;
                case '1':
                    str[4] = 4006002; break;
                default:
                    str[4] = 4006003; break;
            }
            return str;
        }
    }
}
